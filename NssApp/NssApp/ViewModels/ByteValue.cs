using System;
using System.Collections.Generic;
using System.Text;
using ByteSizeLib;

namespace NssApp.ViewModels
{
    public class ByteValue
    {
        public double Bytes { get; private set; }
        public double GB { get { return this.GetValueInUnits(ByteSize.GigaByteSymbol); } }

        private ByteValue(double bytes)
        {
            this.Bytes = bytes;
        }

        public string GetFormattedSize(string format = "0")
        {
            if (Bytes == 0)
                return Bytes.ToString(format);

            return ByteSize.FromBytes(Bytes).ToString(format);
        }

        public string GetFormattedSizeWithRounding()
        {
            if (Bytes == 0)
                return "0";

            var byteSize = ByteSize.FromBytes(Bytes);

            if (byteSize.LargestWholeNumberValue < 10)
                return byteSize.ToString("0.##");
            else if (byteSize.LargestWholeNumberValue < 100)
                return byteSize.ToString("0.#");

            return byteSize.ToString("0");
        }

        public double GetValueInUnits(string units)
        {
            var bs = ByteSize.FromBytes(Bytes);
            switch (units)
            {
                case ByteSize.PetaByteSymbol:
                    return Math.Round(bs.PetaBytes, 2);
                case ByteSize.TeraByteSymbol:
                    return Math.Round(bs.TeraBytes, 2);
                case ByteSize.GigaByteSymbol:
                    return Math.Round(bs.GigaBytes, 2);
                case ByteSize.MegaByteSymbol:
                    return Math.Round(bs.MegaBytes, 2);
                case ByteSize.KiloByteSymbol:
                    return Math.Round(bs.KiloBytes, 2);
                case ByteSize.ByteSymbol:
                case ByteSize.BitSymbol:
                case "":
                    return Math.Round(bs.Bytes, 2);
                default:
                    throw new ArgumentException($"Unknown ByteSize symbol '{units}'");
            }
        }

        public string LargestWholeNumberValue
        {
            get
            {
                return ByteSize.FromBytes(Bytes).LargestWholeNumberValue.ToString("0");
            }
        }

        public string LargestWholeNumberSymbol
        {
            get
            {
                if (Bytes == 0)
                    return string.Empty;

                return ByteSize.FromBytes(Bytes).LargestWholeNumberSymbol;
            }
        }

        public static ByteValue FromTeraBytes(double terabytes)
        {
            var bytes = ByteSize.FromTeraBytes(terabytes).Bytes;

            return new ByteValue(bytes);
        }

        public static ByteValue FromGigaBytes(double gigaBytes)
        {
            var bytes = ByteSize.FromGigaBytes(gigaBytes).Bytes;

            return new ByteValue(bytes);
        }

        public static ByteValue FromBytes(double bytes)
        {
            return new ByteValue(bytes);
        }

        public static ByteValue Zero()
        {
            return new ByteValue(0d);
        }
    }
}

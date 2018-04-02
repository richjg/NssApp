using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NssApp.ViewModels
{
    public static class DataPointExtensions
    {
        public static List<DashboardViewModel.DataSourceItem> AddMonthlyDataPointsAndOrder(this List<DashboardViewModel.RestDataPoint> dataPoints, int months)
        {
            var normalisedDataPoints = new List<DashboardViewModel.DataSourceItem>();
            var maxDate = dataPoints.Any() ? dataPoints.Max(dp => dp.DateTime) : DateTime.UtcNow;

            var maxDataPoint = dataPoints.Any() ? dataPoints.Max(dp => dp.Value) : 0;
            var scaleUnit = ByteValue.FromBytes(maxDataPoint).LargestWholeNumberSymbol;

            for (var i = -months + 1; i <= 0; i++)
            {
                var calculationMonth = maxDate.AddMonths(i);
                var dataForMonth = dataPoints.SingleOrDefault(u => u.DateTime.Month == calculationMonth.Month && u.DateTime.Year == calculationMonth.Year);

                normalisedDataPoints.Add(new DashboardViewModel.DataSourceItem
                {
                    Label = calculationMonth.ToString("MMM"),
                    Value = dataForMonth?.Value.GetValueInScale(scaleUnit) ?? 0
                });
            }

            return normalisedDataPoints;
        }

        public static decimal GetValueInScale(this long bytes, string scaleUnit)
        {
            return (decimal)ByteValue.FromBytes(bytes).GetValueInUnits(scaleUnit);
        }
    }
}

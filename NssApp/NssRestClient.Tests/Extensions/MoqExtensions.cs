using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moq
{
    public static class MoqExtensions
    {
        public static T Configure<T>(this Mock<T> mock, Action<Mock<T>> configureAction)
            where T : class
        {
            configureAction(mock);
            return mock.Object;
        }

        public static T Configure<T>(this Mock<T> mock, List<Action<Mock<T>>> configureActions)
            where T : class
        {
            foreach (var action in configureActions)
            {
                action(mock);
            }
            return mock.Object;
        }
    }
}

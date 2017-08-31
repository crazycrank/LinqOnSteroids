using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqOnSteroids;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testing.Database;
using Testing.Database.Model;

namespace Testing.Runner
{
    [TestClass]
    public class MultipleParameterExpressionTests
    {
        private readonly Expression<Func<Employee, Employee, bool>> _sameYearOfBirth = (e1, e2) => e1.Birthdate.Year == e2.Birthdate.Year;

        [TestMethod]
        public void TestMultipleParameterExpression()
        {
            Employee e1;
            using (var dataContext = new DataContext())
            {
                e1 = dataContext.Employees.First(e => e.Birthdate.Year == 1990);
            }

            using (var dataContext = new DataContext())
            {
                var q = dataContext.Employees.AsExpandable().Where(e => _sameYearOfBirth.Pass(e1, e));
                var result = q.ToList();

                Assert.AreEqual(10, result.Count);
            }
        }
    }
}
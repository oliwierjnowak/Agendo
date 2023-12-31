﻿using Agendo.Server.Persistance;

namespace Agendo.Server.Services
{
    public interface IRightsService
    {
        Task<bool> RightsOverEmp(int emp, int user);
        Task<bool> RightsOverEmps(IEnumerable<int> emps, int user);
    }

    public class RightsService(IRightsRepository _repository) : IRightsService
    {

        public async Task<bool> RightsOverEmp(int emp, int user)
        {
            var right = await _repository.RightsOverEmp(emp, user);
            if(right == null)
            {
                return false;
            }
            if((right.Emp == emp && right.Superior == emp && emp == user) || 
                (right.Emp == emp && right.Superior == user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RightsOverEmps(IEnumerable<int> emps, int user)
        {
            var rights = await _repository.RightsOverEmps(emps, user);
            IEnumerable<int> empValues = rights.Select(employeeRight => employeeRight.Emp);

            bool areEqual = empValues.OrderBy(x => x).SequenceEqual(emps.OrderBy(x => x));
            return areEqual;
        }
    }
}

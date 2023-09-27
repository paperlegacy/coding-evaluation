using System.Collections.Immutable;
using System.Text;

namespace MyOrganization
{
    internal abstract class Organization
    {
        private Position root;

        public Organization()
        {
            root = CreateOrganization();
        }

        protected abstract Position CreateOrganization();

        /**
         * hire the given person as an employee in the position that has that title
         * 
         * @param person
         * @param title
         * @return the newly filled position or empty if no position has that title
         */
        public Position? Hire(Name person, string title)
        {
            bool isRootPosition = root.GetTitle() == title;            
            Random ranNum = new();

            /*
             * using random number with a max range of 10000 to minimize the risk of duplicate Employee Ids for this example...
             * NOT what should be used for a real-world scenario. The Employee constructor requires an int (identifier) when 
             * creating an instance of the object (Employee).
            */
            var newHire = new Employee(ranNum.Next(10000), person);          
            
            // Check the root (Position) to see if the title matches.
            if (isRootPosition && !root.IsFilled())
            {
                root.SetEmployee(newHire);
            }

            return isRootPosition ? root : FindPositionAndAssignEmployee(root.GetDirectReports(), newHire, title);
        }

        /// <summary>
        /// Recursive method to find the correct position, assign the new hire if the position has not been filled, 
        /// or return filled position or null if the position does not exist.
        /// </summary>
        /// <param name="posList"></param>
        /// <param name="newHire"></param>
        /// <param name="title"></param>
        /// <returns>Returns position or null if the position does not exist.</returns>
        private Position? FindPositionAndAssignEmployee(ImmutableList<Position> posList, Employee newHire, string title)
        {
            if (posList != null)
            {
                foreach (Position p in posList)
                {
                    // Position found and has already been filled.
                    if (p.GetTitle() == title && p.IsFilled())
                    {
                        return p;
                    }
                    // Position found and has not been filled, assign the new employee.
                    else if (p.GetTitle() == title && !p.IsFilled())
                    {
                        p.SetEmployee(newHire);
                        return p;
                    }
                    // Continue to dig for the position using recursion.
                    else
                    {
                        FindPositionAndAssignEmployee(p.GetDirectReports(), newHire, title);
                    }
                }
            }
            // Positon has not been found after going through all avaialble positions, return null.
            return null;
        }

        override public string ToString()
        {
            return PrintOrganization(root, "");
        }

        private string PrintOrganization(Position pos, string prefix)
        {
            StringBuilder sb = new StringBuilder(prefix + "+-" + pos.ToString() + "\n");
            foreach (Position p in pos.GetDirectReports())
            {
                sb.Append(PrintOrganization(p, prefix + "  "));
            }
            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleAppsample
{
	class Program
	{
		static List<EmployeeBase> employeeList = new List<EmployeeBase>();

		static void Main(string[] args)
		{
			int option;
			do
			{
				Console.WriteLine("==== Employee Payroll System ====");
				Console.WriteLine("1. Add Employee");
				Console.WriteLine("2. View All Employees");
				Console.WriteLine("3. Calculate and View Salaries");
				Console.WriteLine("4. Save Employees to File");
				Console.WriteLine("5. Load Employees from File");
				Console.WriteLine("6. Calculate Total Payroll");
				Console.WriteLine("7. Exit");
				Console.Write("Select an option: ");

				if (!int.TryParse(Console.ReadLine(), out option))
				{
					Console.WriteLine("Invalid input. Please enter the number.");
					continue;
				}

				Console.Clear();
				switch (option)
				{
					case 1:
						AddEmployee();
						break;
					case 2:
						ViewEmployees();
						break;
					case 3:
						DisplayEmployeeSalaries();
						break;
					case 4:
						SaveEmployeesToFile();
						break;
					case 5:
						LoadEmployeesFromFile();
						break;
					case 6:
						CalculateTotalPayroll();
						break;
					case 7:
						Console.WriteLine("Exiting the program. Goodbye!");
						break;
					default:
						Console.WriteLine("Invalid choice. Try again.");
						break;
				}

				Console.WriteLine();
			} while (option != 7);
		}

		static void AddEmployee()
		{
			Console.Write("Enter Name: ");
			string name = Console.ReadLine();

			Console.Write("Enter ID: ");
			int id = int.Parse(Console.ReadLine());

			Console.Write("Enter Role (Manager/Developer/Intern): ");
			string role = Console.ReadLine();

			Console.Write("Enter Basic Pay: ");
			double basicPay = double.Parse(Console.ReadLine());

			Console.Write("Enter Allowances: ");
			double allowances = double.Parse(Console.ReadLine());

			EmployeeBase employee = role.ToLower() switch
			{
				"manager" => new Manager(name, id, basicPay, allowances),
				"developer" => new Developer(name, id, basicPay, allowances),
				"intern" => new Intern(name, id, basicPay, allowances),
				_ => null
			};

			if (employee != null)
			{
				employeeList.Add(employee);
				Console.WriteLine("Employee added successfully.");
			}
			else
			{
				Console.WriteLine("Invalid role. No employee added.");
			}
		}

		static void ViewEmployees()
		{
			if (employeeList.Count == 0)
			{
				Console.WriteLine("No employees available.");
				return;
			}

			Console.WriteLine("==== Employee List ====");
			foreach (var emp in employeeList)
			{
				Console.WriteLine(emp);
			}
		}

		static void DisplayEmployeeSalaries()
		{
			foreach (var emp in employeeList)
			{
				Console.WriteLine($"{emp.Name} (ID: {emp.ID}) - Salary: {emp.GetSalary():C}");
			}
		}

		static void SaveEmployeesToFile()
		{
			using (StreamWriter writer = new StreamWriter("employees.txt"))
			{
				foreach (var emp in employeeList)
				{
					writer.WriteLine($"{emp.Name},{emp.ID},{emp.Role},{emp.BasicPay},{emp.Allowances}");
				}
			}
			Console.WriteLine("Employees saved to file successfully.");
		}

		static void LoadEmployeesFromFile()
		{
			if (!File.Exists("employees.txt"))
			{
				Console.WriteLine("No saved data found.");
				return;
			}

			employeeList.Clear();
			using (StreamReader reader = new StreamReader("employees.txt"))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					var parts = line.Split(',');
					string name = parts[0];
					int id = int.Parse(parts[1]);
					string role = parts[2];
					double basicPay = double.Parse(parts[3]);
					double allowances = double.Parse(parts[4]);

					EmployeeBase employee = role.ToLower() switch
					{
						"manager" => new Manager(name, id, basicPay, allowances),
						"developer" => new Developer(name, id, basicPay, allowances),
						"intern" => new Intern(name, id, basicPay, allowances),
						_ => null
					};

					if (employee != null)
					{
						employeeList.Add(employee);
					}
				}
			}

			Console.WriteLine("Employees loaded from file successfully.");
		}

		static void CalculateTotalPayroll()
		{
			double totalPayroll = 0;
			foreach (var emp in employeeList)
			{
				totalPayroll += emp.GetSalary();
			}
			Console.WriteLine($"Total Payroll: {totalPayroll:C}");
		}
	}

	abstract class EmployeeBase
	{
		public string Name { get; set; }
		public int ID { get; set; }
		public string Role { get; set; }
		public double BasicPay { get; set; }
		public double Allowances { get; set; }

		protected EmployeeBase(string name, int id, string role, double basicPay, double allowances)
		{
			Name = name;
			ID = id;
			Role = role;
			BasicPay = basicPay;
			Allowances = allowances;
		}

		public virtual double GetSalary()
		{
			return BasicPay + Allowances;
		}

		public override string ToString()
		{
			return $"ID: {ID}, Name: {Name}, Role: {Role}, Basic Pay: {BasicPay:C}, Allowances: {Allowances:C}";
		}
	}

	class Manager : EmployeeBase
	{
		public Manager(string name, int id, double basicPay, double allowances) : base(name, id, "Manager", basicPay, allowances) { }

		public override double GetSalary()
		{
			return base.GetSalary() - 100;
		}
	}

	class Developer : EmployeeBase
	{
		public Developer(string name, int id, double basicPay, double allowances) : base(name, id, "Developer", basicPay, allowances) { }

		public override double GetSalary()
		{
			return base.GetSalary() - 50;
		}
	}

	class Intern : EmployeeBase
	{
		public Intern(string name, int id, double basicPay, double allowances) : base(name, id, "Intern", basicPay, allowances) { }

		public override double GetSalary()
		{
			return base.GetSalary() - 20;
		}
	}
}
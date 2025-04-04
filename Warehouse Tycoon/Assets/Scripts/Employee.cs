using UnityEngine;

public class Employee : MonoBehaviour
{
    public int id; // Unique identifier for the employee
    public string employeeName; // Name of the employee
    public int level; // Level of the employee
    public int exp; // Experience points of the employee
    public int salary; // Salary of the employee
    public int departmentId; // Identifier for the department the employee belongs to
    public EmployeeType.Type employeeType; // Type of the employee (e.g., HR, IT, etc.)
    public DepartmentTypes.Type departmentType; // Type of the department the employee belongs to
    // Constructor to initialize an Employee object
    public Employee() { }
    public Employee(int id, string name, int level, int exp, int salary, int departmentId)
    {
        this.id = id;
        this.name = name;
        this.level = level;
        this.exp = exp;
        this.salary = salary;
        this.departmentId = departmentId;
    }
}
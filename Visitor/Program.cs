using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor
{
    public class Program
    {
        static void Main(string[] args)
        {
            Manager omer = new Manager{ Name = "Ömer", Surname = "DOĞU", Salary = 10000 };
            Manager yasemin = new Manager { Name = "Yasemin", Surname = "Sandal", Salary = 20000 };
            Worker omerWorker= new Worker { Name = "Ömer", Surname = "DOĞU", Salary = 20000 };
            Worker yaseminWorker = new Worker { Name = "Yasemin", Surname = "Sandal", Salary = 50000 };
            omer.Subordinates.Add(yaseminWorker);
            yasemin.Subordinates.Add(yaseminWorker);

            OrganisationalStructure organisationalStructure = new OrganisationalStructure(omer);
            PayrollVisitor payrollVisitor = new PayrollVisitor();
            Payrise payrise = new Payrise();
            organisationalStructure.Accept(payrollVisitor);
            organisationalStructure.Accept(payrise);

        }
    }

    class OrganisationalStructure
    {
        //çalışan enjeksiyonu gerçeleştireceğiz. 
        public EmployeeBase Employee;

        public OrganisationalStructure(EmployeeBase firstEmployee)
        {
            Employee = firstEmployee;
        }
        //temel olarak bir method mevcut. visit işleminin ypacak nesnenin kendisi 
        public void Accept(VisitorBase visitor)
        {
            Employee.Accept(visitor);
        }

    }



    abstract class EmployeeBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal Salary { get; set; }
        public abstract void Accept(VisitorBase visitor);
    }
    class Manager : EmployeeBase
    {
        public Manager(List<EmployeeBase> subordinates)
        {
            Subordinates = subordinates;
        }

        public  List<EmployeeBase> Subordinates { get; set; }
        public override void Accept(VisitorBase visitor)
        {
            visitor.Visit(this);
            foreach (var employee in Subordinates)
            {
                employee.Accept(visitor);//temel visitor temeli oluşturuldu. visitorbase: personel için maaş ödeme zam yapma veya bir kısmı için yapmayı canlandırır. 
                //burada bir visitorbase implemantasyonu ihityaç vardır.

            }
        }
    }
    class Worker : EmployeeBase
    {
        public override void Accept(VisitorBase visitor)
        {
            visitor.Visit(this); //kendisini geçiyoruz. this ile.
        }
    }
    abstract class VisitorBase
    {

        public abstract void Visit(Worker worker);
        public abstract void Visit(Manager manager);
    }
    class PayrollVisitor : VisitorBase
    {
        public override void Visit(Worker worker)
        {
            Console.WriteLine("{0} paid. {1} TL ", worker.Name, worker.Salary);
        }
        public override void Visit(Manager manager)
        {
            Console.WriteLine("{0} paid. {1} ", manager.Name, manager.Salary);
        }
    }
    class Payrise : VisitorBase
    {
        public override void Visit(Worker worker)
        {
            Console.WriteLine("{0} salary increase to {1} ", worker.Name, worker.Salary* (decimal)1,12);
        }

        public override void Visit(Manager manager)
        {
            Console.WriteLine("{0} salary increase to {1} ", manager.Name, manager.Salary*(decimal)1.3);
        }
    }
}


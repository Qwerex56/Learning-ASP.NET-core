using static System.Console;

string[] names = new[] {
    "Michael", "Pam", "Jim", "Dwight",
  "Angela", "Kevin", "Toby", "Creed"
};

WriteLine("Deffered Execution");

// Written using a LINQ extension method
var query1 = names.Where(name => name.EndsWith("m"));

// Fancy syntax, written using LINQ query coprehension syntax
var query2 = from name in names where name.EndsWith("m") select name;

string[] result1 = query1.ToArray();
List<string> result2 = query2.ToList();

IOrderedEnumerable<string> query = names
  .Where(name => name.Length > 4)
  .OrderBy(name => name.Length)
  .ThenBy(name => name);

foreach (var item in query)
{
  WriteLine(item);
  names[2] = "jimmy";
}

WriteLine("Filtering by type");
List<Exception> exceptions = new()
{
  new ArgumentException(),
  new SystemException(),
  new IndexOutOfRangeException(),
  new InvalidOperationException(),
  new NullReferenceException(),
  new InvalidCastException(),
  new OverflowException(),
  new DivideByZeroException(),
  new ApplicationException()
};

IEnumerable<ArithmeticException> arithmeticExceptions = exceptions.OfType<ArithmeticException>();

foreach (var item in arithmeticExceptions) {
  WriteLine(item);
}
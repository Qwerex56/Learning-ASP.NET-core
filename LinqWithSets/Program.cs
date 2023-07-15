using static System.Console;

static void Output(IEnumerable<string> cohort, string description = "") {
  if (!string.IsNullOrEmpty(description)) {
    Write(description);
  }

  Write(" ");
  WriteLine(string.Join(", ", cohort));
  WriteLine();
}

string[] cohort1 = new[]
  { "Rachel", "Gareth", "Jonathan", "George" }; 
string[] cohort2 = new[]
  { "Jack", "Stephen", "Daniel", "Jack", "Jared" }; 
string[] cohort3 = new[]
  { "Declan", "Jack", "Jack", "Jasmine", "Conor" };

Output(cohort1, "Cohort 1");
Output(cohort2, "Cohort 2");
Output(cohort3, "Cohort 3"); 

// No dupes
Output(cohort2.Distinct(), "cohort2.Distinct()");
Output(cohort2.DistinctBy(name => name.Substring(0, 2)), 
  "cohort2.DistinctBy(name => name.Substring(0, 2)):");

// Union
Output(cohort2.Union(cohort3), "cohort2.Union(cohort3)");

// Conscat
Output(cohort2.Concat(cohort3), "cohort2.Concat(cohort3)");

// Intersect
Output(cohort2.Intersect(cohort3), "cohort2.Intersect(cohort3)");

// Except
Output(cohort2.Except(cohort3), "cohort2.Except(cohort3)");

// Zip
Output(cohort1.Zip(cohort2,(c1, c2) => $"{c1} matched with {c2}"), 
  "cohort1.Zip(cohort2)");
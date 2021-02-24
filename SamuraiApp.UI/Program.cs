using System;
using SamuraiApp.Domain;
using SamuraiApp.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {

            //AddVariousTypes();
            //AddSamuraisByName("Alejandro", "Jesus" );
            //GetSamurais("After Add:");
            //QueryFilters();
            //QueryAggregates();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurais();
            //MultipleDatabaseOperations();
            //RetrieveAndDeleteById();
            //QueryAndUpadteBattles_Disconnected();
            //InsertSamuraiWithQuote();
            //InsertSamuraiWithManyQuote();
            //AddQuoteToExistingSamuraiWhiteTracjed();
            //AddQuoteToExistingSamuraiNotTracked(3);
            //Simpler_AddQuoteToExistingSamuraiNotTracked(3);
            //EagerLoadSamuraiWithQuotes();
            //ProjectSamuraiWithQuotes();
            //ExplicitLoadQuotes();
            //FilteringWithRelatedData();
            //ModifyingRelatedDataWhentracked();
            //ModifyingRelatedDataWhenNotTracked();
            //AddingNewSamuraiToAnExistingBattle();
            //ReturnBattleWithSamurais();
            //ReturnAllBattleWithSamurais();
            //AddAllSamuraisToAllBattles();
            //RemoveSamuraiFromABattle();
            //RemoveSamuraiFromABattleExplicit();
            //AddNewSamuraiWithHorse();
            //ReplaceAHorse();
            //GetSamuraiWithHorse();
            //GetSamuraiWithHorse();
            GetWithHorseSamurai();

            Console.WriteLine("Press key");
            Console.ReadKey();

        }

        private static void InsertSamuraiWithQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                   new Quote {Text  = "This is a Quote"}
                }
            };
            _context.Samurais.Add(samurai);
                
            _context.SaveChanges();
        }
        private static void InsertSamuraiWithManyQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                   new Quote {Text  = "This is a method with one quote "},
                   new Quote {Text  = "This is a two quoote"}
                }
            };
            _context.Samurais.Add(samurai);

            _context.SaveChanges();
        }
        private static void AddQuoteToExistingSamuraiWhiteTracjed()
        {
            var samurai = _context.Samurais.FirstOrDefault();


            samurai.Quotes.Add(new Quote
            {
                Text = "This is a method with one quote "

            });
            _context.SaveChanges();
        }
        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you, get me dinner"
            });
            //DbContext in disconnected scenario
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Update(samurai);
                newContext.SaveChanges();
            }
        }
        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var quote = new Quote { Text = "This is simpler dinner ", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }

        private static void AddVariousTypes()
        {
            _context.Samurais.AddRange(
                new Samurai { Name = "Rem" },
                new Samurai { Name = "Emilia" }
                );
            _context.Battles.AddRange(
                new Battle { Name = "Battte agaist white whale" },
                new Battle { Name = "Battle against " });
            _context.SaveChanges();
        }

        private static void AddSamuraisByName(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }

            _context.SaveChanges();
        }
        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais
                .TagWith("Console.app.Program.GetSamurais method")
                .ToList();
            Console.WriteLine($"{text}:  Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void QueryFilters()
        {
            //var name = "Pedro";
            //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
            var coincidence = "J";
            var samurais = _context.Samurais
                .Where(s => EF.Functions.Like(s.Name, coincidence)).ToList();
        }
        private static void QueryAggregates()
        {
            var name = "Pedro";
            var samurais = _context.Samurais.FirstOrDefault(s => s.Name == name);
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault(); 
            samurai.Name += "Ale";
            _context.SaveChanges();
        }

        private static void RetrieveAndUpdateMultipleSamurais() 
        {
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += "Ale");
            _context.SaveChanges();
        }
        private static void MultipleDatabaseOperations()
        {
            var samurais = _context.Samurais.FirstOrDefault();
            samurais.Name += "Ale";
            _context.Samurais.Add(new Samurai { Name = "Shino" });
            _context.SaveChanges();
        }
        private static void RetrieveAndDeleteById()
        {
            var samurai = _context.Samurais.Find(18);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }
        private static void QueryAndUpadteBattles_Disconnected()
        {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconnectedBattles = _context.Battles.ToList();

            }
            disconnectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 01, 01);
                b.EndDate = new DateTime(1570, 12, 1);
            });
            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
    
        }

        private static void  EagerLoadSamuraiWithQuotes()
        {
            //simple query with left join 
            //var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList(); 

            //Query is broken up into multiple queries sent ina single command
            //var samuraiWithQuotes = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();

            var filteredInclude = _context.Samurais
                .Include(s => s.Quotes.Where(q => q.Text.Contains("Dinner"))).ToList();

            //var filteredInclude = _context.Samurais
            //    .Include(s => s.Quotes)
            //    .Include(s=> s.Battles).ToList();
        }

        private static void ProjectSomeProperties()
        {
            //anonnymus properties
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
        }
        public  struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;
        }
        private static void ProjectSamuraiWithQuotes()
        {
            //var somePropsWithQuotes = _context.Samurais
            //    .Select(s => new { s.Id, s.Name, s.Quotes , NumberofQuotes=s.Quotes.Count})
            //    .ToList();

            //var somePropsWithQuotes = _context.Samurais
            //    .Select(s => new { DinnerQuotes = s.Quotes.Where(q => q.Text.Contains("Dinner")) }).ToList();
            var somePropsWithQuotes = _context.Samurais
                .Select(s => new {Samurais=s, DinnerQuotes = s.Quotes.Where(q => q.Text.Contains("Dinner")) }).ToList();
        }

        private static void ExplicitLoadQuotes()
        {
            //make sure there's a horse in the DB then clear the context's change tracker
            _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Friday" });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            //it can only load from a single object

            var samurai = _context.Samurais.Find(1);
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();

            //filtering
            //var dinnerQuote = _context.Entry(samurai)
            //    .Collection(a => a.Quotes)
            //    .Query().
            //    Where(q=> q.Text.Contains("Dinner")).ToList();


        }
        private static void FilteringWithRelatedData()
        {
            //retrieve any samurai that has ever used the workd happy in a quote
            var samurais = _context.Samurais
                .Where(s => s.Quotes.Any(q => q.Text.Contains("Dinner")))
                .ToList();
        }

        private static void ModifyingRelatedDataWhentracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                .FirstOrDefault(s => s.Id == 3);
            samurai.Quotes[0].Text = "Didn't u see that coming ?";
            //_context.Quotes.Remove(samurai.Quotes[3]);
            _context.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                .FirstOrDefault(s => s.Id == 3);
            var quote = samurai.Quotes[0];
            quote.Text += "Didn't u see that coming ?";

            using var newContext = new SamuraiContext();
            //newContext.Quotes.Update(quote);
            newContext.Entry(quote).State = EntityState.Modified;
            newContext.SaveChanges();
        }

        ////may to many relationship
        ///
        private static void AddingNewSamuraiToAnExistingBattle() 
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.Samurais.Add(new Samurai { Name = "Kirito" });
            _context.SaveChanges(); 
        }

        private static void ReturnBattleWithSamurais()
        {
            var battle = _context.Battles.Include(b => b.Samurais).FirstOrDefault();
            
        }
        private static void ReturnAllBattleWithSamurais()
        {
            var battle = _context.Battles.Include(b => b.Samurais).ToList();

        }

        private static  void AddAllSamuraisToAllBattles()
        {
            var allBattles = _context.Battles.Include(b=> b.Samurais).ToList();
            var allSamurais = _context.Samurais.Where(s=>s.Id!=26).ToList();
            foreach(var battle in allBattles)
            {
                battle.Samurais.AddRange(allSamurais);
            }
            _context.SaveChanges();
        }

        private static void RemoveSamuraiFromABattle()
        {
            var battleWithSamurai = _context.Battles
                    .Include(b => b.Samurais.Where(s => s.Id == 1))
                    .Single(s => s.BattleId == 2);
            var samurai = battleWithSamurai.Samurais[0];
            battleWithSamurai.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void WillNotRemoveSamuraiFromABattle()
        {
            var battle = _context.Battles.Find(4);
            var samurai = _context.Samurais.Find(4);
            battle.Samurais.Remove(samurai);
            _context.SaveChanges();//the relationship is not tracked
        }


        private static void RemoveSamuraiFromABattleExplicit()
        {
            //single throws a exceptions and singleordef returns a null if not true  
            var b_s = _context.Set<SamuraiBattle>()
                .SingleOrDefault(bs => bs.BattleId == 1 && bs.SamuraiId == 10);

            if (b_s != null)
            {
                _context.Remove(b_s);//.Set<SamuraiBattle>().Remove works, too
                _context.SaveChanges();

            }
        }


        //---------one to one relation ship

        private static void AddNewSamuraiWithHorse()
        {
            var samurai = new Samurai { Name = "Kakashi sensei" };
            samurai.Horse = new Horse { Name = "Silvva" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddNewHorseToSamuraiUsingId()
        {
            
            var horse = new Horse { Name = "Tiro al blanco", SamuraiId= 5 };
            _context.Add(horse);
            _context.SaveChanges();
        }
        private static void AddNewHorseToSamuraiObject()
        {

            var samurai = _context.Samurais.Find(12);
            samurai.Horse = new Horse { Name = "Bella" };
            
            _context.SaveChanges();
        }

        private static void AddNewHorseToDisconnectedSamuraiObject()
        {
            var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 7);
            samurai.Horse = new Horse { Name = "Mr Ed" };

            using var newContext = new SamuraiContext();
            newContext.Samurais.Attach(samurai);
            newContext.SaveChanges();
        }

        private static void ReplaceAHorse()
        {
            //this will delete the existing row and create a new one 
            //var samurai = _context.Samurais.Include(s => s.Horse)
            //    .FirstOrDefault(s => s.Id == 5);
            //samurai.Horse = new Horse { Name = "Trigger" };
            //_context.SaveChanges();

            var horse = _context.Set<Horse>().FirstOrDefault(h => h.Name == "Mr. Bond");
            horse.SamuraiId = 5;//owns trigger
            _context.SaveChanges();

                
        }

        private static void GetSamuraiWithHorse()
        {
            var samurais = _context.Samurais.Include(s => s.Horse).ToList();
        }
        private static void GetWithHorseSamurai()
        {
            var horseOnly = _context.Set<Horse>().Find(3);
            var horseWithSamurai = _context.Samurais.Include(s => s.Horse)
                .FirstOrDefault(s => s.Horse.Id == 3);

            var horseSamuraiPairs = _context.Samurais
                .Where(s => s.Horse != null)
                .Select(s => new { Horse = s.Horse, Samurai = s })
                .ToList();


             }

    }
}

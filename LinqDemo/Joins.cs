using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel;

namespace LinqDemo
{
    /// <remarks>Derived from an MSDN page on Enumerable.GroupJoin</remarks>
    [LinqDemoClass]
    public class Joins
    {
        #region Types
        class Person
        {
            public string Name { get; set; }
        }

        class Pet
        {
            public string Name { get; set; }
            public Person Owner { get; set; }
        }
        #endregion Types

        [LinqDemoMethod]
        [Description("Demonstration of Linq's Join() and GroupJoin() methods")]
        public static void DemoJoins()
        {
            Util.Entering();

            demoJoin();
            Console.WriteLine();
            demoGroupJoin();
        }

        private static void getPetData(out List<Person> people, out List<Pet> pets) {
            Person magnus = new Person { Name = "Hedlund, Magnus" };
            Person terry = new Person { Name = "Adams, Terry" };
            Person charlotte = new Person { Name = "Weiss, Charlotte" };
            people = new List<Person> {  magnus, terry, charlotte };

            Pet barley = new Pet { Name = "Barley", Owner = terry };
            Pet boots = new Pet { Name = "Boots", Owner = terry };
            Pet whiskers = new Pet { Name = "Whiskers", Owner = charlotte };
            Pet daisy = new Pet { Name = "Daisy", Owner = magnus };
            pets = new List<Pet> { barley, boots, whiskers, daisy };
        }

        private static void demoGroupJoin()
        {
            Console.WriteLine("...Entering {0:s}...", Util.GetCurrentMethodName());
            List<Person> people;
            List<Pet> pets;
            getPetData(out people, out pets);

            // Create a list of anonymous type that is a pair: { OwnerName, Pets }
            var ownerAndPetsQuery =
                people.GroupJoin(pets,
                    person => person,  // One group for each of these equalling next line
                    pet => pet.Owner,  // One group for each of these equalling prev line
                    (person, petCollection) => new {
                                         OwnerName = person.Name,
                                         Pets = petCollection.Select(pet => pet.Name)
                                     });

            foreach (var ownerAndPets in ownerAndPetsQuery)
            {
                Console.WriteLine("Owner - {0:s}:" + Environment.NewLine + "\tPets: {1:s}",
                    ownerAndPets.OwnerName,                 // Owner's name
                    String.Join(", ", ownerAndPets.Pets)    // Owner's pet's names
                    );
            }
        }

        private static void demoJoin()
        {
            Console.WriteLine("...Entering {0:s}...", Util.GetCurrentMethodName());

            List<Person> people;
            List<Pet> pets;
            getPetData(out people, out pets);
            var ownerAndPetQuery =
                people.Join(pets,
                    person => person,
                    pet => pet.Owner,
                    (person, pet) => new { OwnerName = person.Name, Pet = pet.Name });
            foreach (var ownerAndPet in ownerAndPetQuery)
            {
                Console.WriteLine("Owner={0:s}; Pet={1:s}", ownerAndPet.OwnerName, ownerAndPet.Pet);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();

            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }


        //// TODO Items: ////
        
        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            switch(crudOperation)
            {
                case "create":
                    CreateNewEmployee(employee);
                    break;
                case "delete":
                    DeleteEmployee(employee);
                    break;
                case "read":
                    DisplayEmployee(employee);
                    break;
                case "update":
                    UpdateEmployee(employee);
                    break;
                default:
                    UserInterface.DisplayUserOptions("Input not recognized.");
                    break;
            }
        }
        internal static void CreateNewEmployee(Employee employee)
        {
            Employee employeeToCreate = db.Employees.Where(e => e.FirstName == employee.FirstName && e.LastName == employee.LastName && e.EmployeeNumber == employee.EmployeeNumber && e.Email == employee.Email).FirstOrDefault();
            if (employeeToCreate == null)
            {
                db.Employees.InsertOnSubmit(employee);
                db.SubmitChanges();
            }
            else
            {
                throw new Exception();
            }
        }
        internal static void DeleteEmployee(Employee employee)
        {
            Employee employeeToDelete = db.Employees.Where(e => e.LastName == employee.LastName && e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();
            if (employeeToDelete != null)
            {
                db.Employees.DeleteOnSubmit(employeeToDelete);
                db.SubmitChanges();
            }
            else
            {
                throw new Exception();
            }
        }
        internal static void DisplayEmployee(Employee employee)
        {
            Employee employeeToRead = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();
            if (employeeToRead != null)
            {
                UserInterface.DisplayEmployeeInfo(employeeToRead);
            }
            else
            {
                throw new Exception();
            }
        }
        internal static void UpdateEmployee(Employee employee)
        {
            Employee employeeToUpdate = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();
            if (employeeToUpdate != null)
            {
                employeeToUpdate.FirstName = UserInterface.GetStringData("new first name", "the employee's");
                employeeToUpdate.LastName = UserInterface.GetStringData("new last name", "the employee's");
                employeeToUpdate.EmployeeNumber = int.Parse(UserInterface.GetStringData("new employee number", "the employee's"));
                employeeToUpdate.Email = UserInterface.GetStringData("new email", "the employee's");
                db.SubmitChanges();
            }
            else
            {
                throw new Exception();
            }
        }

        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            Room openRoom = db.Rooms.Where(r => r.AnimalId == null).FirstOrDefault();
            if(openRoom != null)
            {
                openRoom.AnimalId = animal.AnimalId;
                db.Animals.InsertOnSubmit(animal);
                db.SubmitChanges();
            }
            else
            {
                Console.WriteLine("All rooms are full!");
            }
        }    
        internal static Animal GetAnimalByID(int id)
        {
            Animal AnimalToRead = db.Animals.Where(e => e.AnimalId == id).FirstOrDefault();
            return AnimalToRead;
        }       
        internal static void UpdateAnimalWithCsv(Animal animal)
        {
            String[] values = File.ReadAllText(@"C:\Users\Patrick\Documents\Development\devCodeCamp\Week_07\HumaneSociety\csvToLinq.csv").Split(',');
            List<string> valueList = values.OfType<string>().ToList();

            Animal animalToUpdate = db.Animals.Where(e => e.AnimalId == animal.AnimalId).FirstOrDefault();

            for (int i = 0; i < valueList.Count; i++)
            {
                switch (i)
                {
                    case 1:
                        animalToUpdate.CategoryId = Convert.ToInt32(valueList[1]);
                        break;
                    case 2:
                        animalToUpdate.Name = valueList[2];
                        break;
                    case 3:
                        animalToUpdate.Age = Convert.ToInt32(valueList[3]);
                        break;
                    case 4:
                        animalToUpdate.Demeanor = valueList[4];
                        break;
                    case 5:
                        animalToUpdate.KidFriendly = Convert.ToBoolean(valueList[5]);
                        break;
                    case 6:
                        animalToUpdate.PetFriendly = Convert.ToBoolean(valueList[6]);
                        break;
                    case 7:
                        animalToUpdate.Weight = Convert.ToInt32(valueList[7]);
                        break;
                    default:
                        break;
                }
            }
            db.SubmitChanges();
        }
        internal static void UpdateAnimal(Animal animal, Dictionary<int, string> updates)
        {
            Animal animalToUpdate = db.Animals.Where(e => e.AnimalId == animal.AnimalId).FirstOrDefault();
            
            foreach ( int key in updates.Keys)
            {
                var value = updates[key];

                switch (key)
                {
                    case 1:
                        animalToUpdate.CategoryId = db.Categories.Where(e => e.Name == value).Select(c => c.CategoryId).FirstOrDefault();
                        break;
                    case 2:
                        animalToUpdate.Name = value;
                        break;
                    case 3:
                        animalToUpdate.Age = Convert.ToInt32(value);
                        break;
                    case 4:
                        animalToUpdate.Demeanor = value;
                        break;
                    case 5:
                        animalToUpdate.KidFriendly = Convert.ToBoolean(value);
                        break;
                    case 6:
                        animalToUpdate.PetFriendly = Convert.ToBoolean(value);
                        break;
                    case 7:
                        animalToUpdate.Weight = Convert.ToInt32(value);
                        break;
                    default:
                        break;
                }
            }
            db.SubmitChanges();
        }

        internal static void RemoveAnimal(Animal animal)
        {
            Animal AnimalToDelete = db.Animals.Where(e => e.AnimalId == animal.AnimalId).FirstOrDefault();
            db.Animals.DeleteOnSubmit(AnimalToDelete);
            Room roomToEmpty = db.Rooms.Where(r => r.AnimalId == animal.AnimalId).FirstOrDefault();
            roomToEmpty.AnimalId = null;
            db.SubmitChanges();
        }

        // TODO: Animal Multi-Trait Search
       internal static IQueryable<Animal> SearchForAnimalByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
       {
            var results = new List<Animal>();
            foreach(Animal animal in db.Animals)
            {
                results.Add(animal);
            }
            if(updates.ContainsKey(1))
            {
                results.RemoveAll(a => a.CategoryId != Int32.Parse(updates[1]));
            }
            if(updates.ContainsKey(2))
            {
                results.RemoveAll(a => a.Name != updates[2]);
            }
            if(updates.ContainsKey(3))
            {
                results.RemoveAll(a => a.Age != Int32.Parse(updates[3]));
            }
            if(updates.ContainsKey(4))
            {
                results.RemoveAll(a => a.Demeanor != updates[4]);
            }
            if(updates.ContainsKey(5))
            {
                results.RemoveAll(a => a.KidFriendly != bool.Parse(updates[5]));
            }
            if(updates.ContainsKey(6))
            {
                results.RemoveAll(a => a.PetFriendly != bool.Parse(updates[6]));
            }
            if(updates.ContainsKey(7))
            {
                results.RemoveAll(a => a.Weight != Int32.Parse(updates[7]));
            }
            if(updates.ContainsKey(8))
            {
                results.RemoveAll(a => a.AnimalId != Int32.Parse(updates[8]));
            }
            var queryList = results.AsQueryable();
            return queryList;
       }

        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {
            Category category = db.Categories.Where(e => e.Name == categoryName.ToLower()).FirstOrDefault();
            if (category != null)
            {
                return category.CategoryId;
            }
            else
            {
                Category newCategory = new Category { Name = categoryName };
                db.Categories.InsertOnSubmit(newCategory);
                db.SubmitChanges();
                category = db.Categories.Where(e => e.Name == categoryName).FirstOrDefault();
                return category.CategoryId;
            }
        }
        
        internal static Room GetRoom(int animalId)
        {            
            Room animalRoom = db.Rooms.Where(e => e.AnimalId == animalId).FirstOrDefault();
            return animalRoom;
        }

        internal static int GetDietPlanId(string dietPlanName)
        {
            switch (dietPlanName.ToLower())
            {
                case "cat":
                    goto case "cat food";
                case "cat food":
                    DietPlan catDietPlan = db.DietPlans.Where(e => e.Name == "Cat Food").FirstOrDefault();
                    return catDietPlan.DietPlanId;
                case "dog":
                    goto case "dog food";
                case "dog food":
                    DietPlan dogDietPlan = db.DietPlans.Where(e => e.Name == "Dog Food").FirstOrDefault();
                    return dogDietPlan.DietPlanId;
                case "bird":
                    goto case "bird food";
                case "bird food":
                    DietPlan birdDietPlan = db.DietPlans.Where(e => e.Name == "Bird Food").FirstOrDefault();
                    return birdDietPlan.DietPlanId;
                case "pig":
                    goto case "pig food";
                case "pig food":
                    DietPlan pigDietPlan = db.DietPlans.Where(e => e.Name == "Pig Food").FirstOrDefault();
                    return pigDietPlan.DietPlanId;
                case "rabbit":
                    goto case "rabbit food";
                case "rabbit food":
                    DietPlan rabbitDietPlan = db.DietPlans.Where(e => e.Name == "Rabbit Food").FirstOrDefault();
                    return rabbitDietPlan.DietPlanId;
                default:
                    return 0;
            }
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            if (animal.AdoptionStatus != "approved")
            {
                Adoption adoption = new Adoption()
                {
                    ClientId = client.ClientId,
                    AnimalId = animal.AnimalId,
                    ApprovalStatus = "awaiting",
                    AdoptionFee = 75,
                    PaymentCollected = true,
                };
                db.Adoptions.InsertOnSubmit(adoption);
                db.SubmitChanges();
            }
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            var adoptions = db.Adoptions.Where(a => a.ApprovalStatus == "awaiting");
            return adoptions;
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            switch(isAdopted)
            {
                case true:
                    ApproveAdoption(adoption);
                    break;
                case false:
                    adoption.ApprovalStatus = "denied";
                    RemoveAdoption(adoption.AnimalId, adoption.ClientId);
                    break;
            }
            db.SubmitChanges();
        }
        internal static void ApproveAdoption(Adoption adoption)
        {
            adoption.ApprovalStatus = "approved";
            Animal animalToAdopt = db.Animals.Where(a => a.AnimalId == adoption.AnimalId).FirstOrDefault();
            if (adoption.PaymentCollected == true)
            {
                RemoveAdoption(adoption.AnimalId, adoption.ClientId);
                RemoveAnimal(animalToAdopt);
            }
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            Adoption adoptionToRemove = db.Adoptions.Where(a => a.AnimalId == animalId && a.ClientId == clientId).FirstOrDefault();
            if(adoptionToRemove != null)
            {
                db.Adoptions.DeleteOnSubmit(adoptionToRemove);
                db.SubmitChanges();
            }
        }

        //TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            var shots = db.AnimalShots.Where(s => s.AnimalId == animal.AnimalId);
            return shots;
        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            Shot shotToAdd = db.Shots.Where(s => s.Name == shotName).FirstOrDefault();
            var actualShot = new AnimalShot { AnimalId = animal.AnimalId, ShotId = shotToAdd.ShotId, DateReceived = DateTime.Today };
            db.AnimalShots.InsertOnSubmit(actualShot);
            db.SubmitChanges();
        }
    }
}
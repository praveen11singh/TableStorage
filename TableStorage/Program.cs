using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace TableStorage
{

    class Patient : TableEntity
    {
        public string PatientName { get; set; }
        public string PaitentAge { get; set; }
        public string Medication { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1 :- Create a connection with the storage account
            CloudStorageAccount storageAccount
                = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=arena11storage;AccountKey=jV1YbiOkX06nmDyNkyJvl5raNMeGa+URoQc+g7BaY5CAMv4gRWer5hwhgvjrI/8VhuSgCtZvRiTekjqFZWxv1Q==;EndpointSuffix=core.windows.net");
            // Step 2 :-- Table client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Step 3 :- Get reference to hospital management table
            CloudTable table1 = tableClient.GetTableReference("HospitalManagementSystem");



            // get the object to be updated
            TableOperation retrieveOperation = TableOperation.
                            Retrieve<Patient>("Patient", "P1001");
            TableResult retrievedResult = table1.Execute(retrieveOperation);

            Patient updateEntity = (Patient)retrievedResult.Result;
            updateEntity.PatientName = "PK";
            // attach this object with the operation
            TableOperation updateOperation = TableOperation.Replace(updateEntity);
            //TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
            // execute thate operation using the table
            table1.Execute(updateOperation);


            //TableBatchOperation batchOperation = new TableBatchOperation();

            // Create a customer entity and add it to the table.
            //CustomerEntity customer1 = new CustomerEntity("Smith", "Jeff");
            //customer1.Email = "Jeff@contoso.com";
            //customer1.PhoneNumber = "425-555-0104";

            //// Create another customer entity and add it to the table.
            //CustomerEntity customer2 = new CustomerEntity("Smith", "Ben");
            //customer2.Email = "Ben@contoso.com";
            //customer2.PhoneNumber = "425-555-0102";

            //// Add both customer entities to the batch insert operation.
            //batchOperation.Insert(customer1);
            //batchOperation.Insert(customer2);

            //// Execute the batch operation.
            //table.ExecuteBatch(batchOperation);


            ////prepare the object --create
            //Patient newpat = new Patient();
            //newpat.PatientName = "newpat2";
            //newpat.RowKey = "P1001";
            //newpat.PartitionKey = "Patient";
            //newpat.PaitentAge = "40";
            //newpat.Medication = "Tablets";
            //// you will attach it to a operation
            //TableOperation insertoperation = TableOperation.Insert(newpat);
            //// send this operation to the table
            //table1.Execute(insertoperation);

            // Step 4 :- Create table Query where we will filter 
            // only patient partition
            TableQuery<Patient> MyQuery = new TableQuery<Patient>().
                                    Where(
                                    TableQuery.CombineFilters(
                                    TableQuery
                                    .GenerateFilterCondition("PartitionKey",
                                        QueryComparisons.Equal
                                        , "Patient"),
                                    TableOperators.And,
                                    TableQuery
                                    .GenerateFilterCondition("RowKey",
                                        QueryComparisons.Equal
                                        , "P1001")));
            // Step 5 :- Fire the query and loop through collection.
            foreach (Patient x in table1.ExecuteQuery<Patient>(MyQuery))
            {
                Console.WriteLine(x.PatientName + "  " + x.Medication);
            }
            Console.Read();
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Azure.Data.Tables;
using System.Linq;

namespace EA.KodTest
{
    /// <summary>
    /// GetPackageMeasures Azure Function class
    /// </summary>
    public static class GetPackageMeasures
    {
        /// <summary>
        /// GetPackageMeasures Azure Function
        /// </summary>
        /// <param name="req">HttpRequest object</param>
        /// <param name="packageTableClient">Table client used for accessing Azure Storage</param>
        /// <param name="log">Logging interface</param>
        /// <returns></returns>
        [FunctionName("GetPackageMeasures")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req,
            [Table("Package")] TableClient packageTableClient,
            ILogger log)
        {
            // Make sure test data exists (for the purpose of this code test)
            await CreateTestData(packageTableClient);

            // Log trace info
            log.LogTrace($"QueryString = {req.QueryString}");

            try
            {
                // Fetch the package number from the query string and validate the value
                var packageNumber = req.Query["packagenumber"];
                EnsureInputIsValid(packageNumber);

                // Try to find a package in the table storage with a matching number
                var package = await packageTableClient.QueryAsync<Package>(p => p.Number == packageNumber.ToString()).FirstOrDefaultAsync();
                if (package == null)
                {
                    // No match found
                    log.LogDebug("Found no package with a matching number");
                    return new NotFoundResult();
                }

                // Found a match - return status code 200 with the data
                log.LogDebug("Found a package with a matching number");
                var dto = new PackageDto(package);
                return new OkObjectResult(dto);
            }
            catch (ArgumentException e)
            {
                // The given package number is invalid - return status code 400 with some info
                log.LogDebug(e, "Recieved a Bad Request");
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                // An unhandled exception was thrown - return status code 500
                log.LogError(e, "An Internal Server Error was encountered");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Validate the input values (package number) and throw an exception if invalid
        /// </summary>
        /// <param name="packageNumber">Requested package number</param>
        /// <exception cref="ArgumentNullException">The package number is NULL.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The package number is of the wrong length or format.</exception>
        private static void EnsureInputIsValid(string packageNumber)
        {
            if (packageNumber == null)
                throw new ArgumentNullException(nameof(packageNumber), "Missing or empty value");
            if (packageNumber.Length != 18)
                throw new ArgumentOutOfRangeException(nameof(packageNumber), "Invalid length");
            if (!new Regex("^\\d+$").IsMatch(packageNumber))
                throw new ArgumentOutOfRangeException(nameof(packageNumber), "Invalid format");
        }

        #region TestData
        private static async Task CreateTestData(TableClient packageTableClient)
        {
            var testPackageNumber = "123456789012345678";

            var exists = await packageTableClient.QueryAsync<Package>(p => p.Number == testPackageNumber).AnyAsync();
            if (!exists)
            {
                var testPackage = new Package
                {
                    Number = testPackageNumber,
                    Weight = 7.489,
                    Length = 33.6,
                    Height = 67.5,
                    Width = 28.1,

                    PartitionKey = "HTTP",
                    RowKey = Guid.NewGuid().ToString()
                };

                await packageTableClient.UpsertEntityAsync(testPackage);
            }
        }
        #endregion
    }
}

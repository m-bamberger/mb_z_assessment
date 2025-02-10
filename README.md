# Documentation

## Implementation Informations
- As we wrote in the email, I assumed that several instances of the application (possibly on several servers) 
share the same database.

- For the decrement-stock and addstock i decieded to allow negative stocks too, because in an production envirnoment it could mean, 
that you have to produce additional product, when your stock is negative.

- I decided to create the ProductService between the conroller and the repository, because i don't wanted the calculation
 for the add-stock and decrease-stock endpoints, directly in the controller. Without them i could inject and use the repository directly in the controller.

- The logger currently logs only to the console. It could be extendet that the application logs to ElasticSearch.

- For the Unit-Test is only covered the most important methods (in the ProductRepository) because the productService and the
 productController only passthrough the data to the repository. 

- First i addes the solution in Azure DevOps and added a build and test pipline.
I testted the released application on my IIS-Server (with multiple instances).
For sharing with you its easier with permissions in Github instead of Azure DevOps.

- Possible (not implemented) extensions:
  - Authentication with API-Key oder with login and token
  - lLgging to elastic search 
  - Caching for faster data access and to reduce the load on the database
  - When the muliple instances of the application dont share the same db: Extra service/application for the id generation.


## Overview

The `ProductsController` class in the `ZeissAssessment` project is an ASP.NET Core Web API controller that manages 
product-related operations. It provides endpoints to perform CRUD operations on products, as well as additional functionalities 
like stock management and product search.

## Endpoints

### Get All Products
- **URL:** `/api/products`
- **Method:** GET
- **Description:** Retrieves a list of all products.
- **Responses:**
  - `200 OK`: Returns a list of `ProductDto`.

### Get Product by ID
- **URL:** `/api/products/{id}`
- **Method:** GET
- **Description:** Retrieves a single product by its ID.
- **Parameters:**
  - `id` (int): The ID of the desired product.
- **Responses:**
  - `200 OK`: Returns the `ProductDto` with the specified ID.
  - `404 Not Found`: If the product with the specified ID does not exist.

### Add New Product
- **URL:** `/api/products`
- **Method:** POST
- **Description:** Adds a new product.
- **Request Body:** `AddProductRequestDto`
- **Responses:**
  - `201 Created`: Returns the added `ProductDto` with a link to the new entity.

### Update Product
- **URL:** `/api/products/{id}`
- **Method:** PUT
- **Description:** Updates an existing product.
- **Parameters:**
  - `id` (int): The ID of the product to update.
- **Request Body:** `ProductDto`
- **Responses:**
  - `200 OK`: Returns the updated `ProductDto`.
  - `400 Bad Request`: If the ID in the URL does not match the ID in the request body.
  - `404 Not Found`: If the product with the specified ID does not exist.

### Delete Product
- **URL:** `/api/products/{id}`
- **Method:** DELETE
- **Description:** Deletes a product by its ID.
- **Parameters:**
  - `id` (int): The ID of the product to delete.
- **Responses:**
  - `204 No Content`: If the product was successfully deleted.
  - `404 Not Found`: If the product with the specified ID does not exist.

### Decrement Stock
- **URL:** `/api/products/{id}/decrement-stock/{quantity}`
- **Method:** PUT
- **Description:** Decreases the stock of a product by a specified quantity.
- **Parameters:**
  - `id` (int): The ID of the product.
  - `quantity` (int): The quantity to decrement.
- **Responses:**
  - `200 OK`: Returns the updated `ProductDto`.
  - `404 Not Found`: If the product with the specified ID does not exist.

### Add to Stock
- **URL:** `/api/products/{id}/add-to-stock/{quantity}`
- **Method:** PUT
- **Description:** Increases the stock of a product by a specified quantity.
- **Parameters:**
  - `id` (int): The ID of the product.
  - `quantity` (int): The quantity to add.
- **Responses:**
  - `200 OK`: Returns the updated `ProductDto`.
  - `404 Not Found`: If the product with the specified ID does not exist.

### Search Products by Name
- **URL:** `/api/products/search?name={name}`
- **Method:** GET
- **Description:** Searches for products by name.
- **Parameters:**
  - `name` (string): The name to search for.
- **Responses:**
  - `200 OK`: Returns a list of `ProductDto` that match the search criteria.
  - `204 No Content`: If no products match the search criteria.

### Get Products by Stock Level
- **URL:** `/api/products/stock-level?min={min}&max={max} 
- **Method:** GET
- **Description:** Retrieves products within a specified stock level range.
- **Parameters:**
  - `min` (int?): The minimum stock level.
  - `max` (int?): The maximum stock level.
- **Responses:**
  - `200 OK`: Returns a list of `ProductDto` within the specified stock level range.
  - `400 Bad Request`: If `min` or `max` is not provided.
  - `204 No Content`: If no products match the stock level criteria.

## Running Locally

To run the project locally, you need to update the connection string in the `appsettings.json` file to point to your local database.
 Here is an example of how to update the connection string:
```json
 "ConnectionStrings": { "ZeissAssessmentConnetcionString": "Server=your_local_server;Database=your_local_database;Trusted_Connection=True;MultipleActiveResultSets=true" } }
```
Replace `your_local_server` and `your_local_database` with your local database server name and database name, respectively.

After this you can execute the following command in the NuGet Package-Manager-Console(Visual Studio) to apply the migration-scripts and create the database and the tables.

```
Update-Database
```

If you want do migrate the database with the terminal you have to executethe following commands in the projects directory:

```
dotnet tool install --global dotnet-ef
```
```
dotnet ef database update
```

Note: I my tests i faced the problem twice that the build wasn't possible. Restart of Visual Studio and clean the solution helped me out. 

## Azure DevOps Buildpipeline

The `Buildpipeline.yml` file contains the YAML code for Azure DevOps to build and test the application. 
When you want to test this, you may need to edit the YAML file and change the pool name to your own agent pool.

```yml
...

pool:
  vmImage: 'windows-latest'
  name: 'Default'

...
```
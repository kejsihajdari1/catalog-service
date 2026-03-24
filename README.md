# Beer Collection API

A production-ready ASP.NET Core Web API for managing a beer collection with Entity Framework Core and RESTful design.

## Features

- Create, list, and search beers (name, type, optional rating 1-5)
- Automated rating averaging system
- Swagger/OpenAPI documentation
- Async operations with cancellation support
- Global error handling and structured logging
- Relational database design with proper foreign keys

## Quick Start

```bash
# Run the API
cd CatalogService
dotnet run
```

**API**: `http://localhost:5266`  
**Swagger UI**: `http://localhost:5266/swagger`

```bash
# Run tests
dotnet test
```

## API Endpoints

### Create Beer

`POST /api/beers`

```json
{ "name": "Guinness", "type": "Stout", "rating": 5 }
```

### List/Search Beers

`GET /api/beers` - List all  
`GET /api/beers?search=pale` - Search by name

### Update Rating

`PUT /api/beers/{id}/rating`

```json
{ "rating": 4 }
```

Rating is added to existing ratings and automatically averaged.

## Technology Stack

- ASP.NET Core Web API (.NET 10.0)
- Entity Framework Core with SQLite
- Dependency Injection & Service Layer pattern
- Data Annotations for validation
- xUnit + InMemory Database for testing
- Extension methods for clean mapping

## Client Example

```javascript
// Create beer
const response = await fetch("http://localhost:5266/api/beers", {
  method: "POST",
  headers: { "Content-Type": "application/json" },
  body: JSON.stringify({ name: "IPA", type: "Pale Ale", rating: 4 }),
});

// Search beers
const beers = await fetch("http://localhost:5266/api/beers?search=pale").then(
  (res) => res.json(),
);
```

## Database

SQLite with two tables: `Beers` and `Ratings`. Database auto-created on first run. Proper foreign keys ensure data integrity and eliminate concurrency issues.

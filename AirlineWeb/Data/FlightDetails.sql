SELECT TOP (1000)
    [Id]
      , [FlightNumber]
      , [Price]
FROM [AirlineDb].[dbo].[FlightDetails];

-- Remove all records from the FlightDetails table
DELETE FROM [AirlineDb].[dbo].[FlightDetails];
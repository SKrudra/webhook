SELECT TOP (1000)
    [Id]
      , [Secret]
      , [Publisher]
FROM [TravelAgentDb].[dbo].[WebhookSecrets]

-- insert a sample webhook secret with:
-- secret df8b1d9b-5ca5-4b6d-8453-1e067294581f
-- publisher PAN America
-- Get these values from the webhook registration web page from the AirlineWebhook project at http://localhost:5052/index.html
INSERT INTO [TravelAgentDb].[dbo].[WebhookSecrets]
    ([Secret]
    ,[Publisher])
VALUES
    ( 'df8b1d9b-5ca5-4b6d-8453-1e067294581f'
           , 'PAN America')
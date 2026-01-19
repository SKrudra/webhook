SELECT TOP (1000)
  [Id]
      , [WebhookUrl]
      , [Secret]
      , [WebhookType]
      , [WebhookPublisher]
FROM [AirlineDb].[dbo].[WebhookSubscriptions];

-- delete all data from webhook subscriptions
DELETE FROM [AirlineDb].[dbo].[WebhookSubscriptions];
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;

namespace Lambda;

public class Handler
{
    private const string tableName = "dotnet_data";
    private readonly IAmazonDynamoDB _client;


    public Handler() : this(null)
    { }

    public Handler(IAmazonDynamoDB? dynamoDbClient)
    {
        _client = dynamoDbClient ?? new AmazonDynamoDBClient();
    }

    public async Task<Data?> HandleAsync(Event @event, ILambdaContext context)
    {
        var id = @event?.PathParameters?.GetValueOrDefault("id");

        var getAction = new GetItemRequest
        {
            TableName = tableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                ["ID"] = new AttributeValue
                {
                    S = id
                }

            }
        };

        try
        {
            var getItemResponse = await _client.GetItemAsync(getAction);

            if (getItemResponse.Item == null)
            {
                return null;
            }

            var data = new Data(
                getItemResponse.Item["ID"].S,
                getItemResponse.Item["Title"].S,
                decimal.Parse(getItemResponse.Item["Rating"].N),
                getItemResponse.Item["Genres"].SS.ToArray());

            return data;
        }
        catch (Exception exception)
        {
            context.Logger.LogError(exception, "Error");
        }

        return null;
    }
}
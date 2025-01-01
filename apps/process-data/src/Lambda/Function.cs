using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Lambda;

public class Function
{

    private const string tableName = "dotnet_data";
    private IAmazonDynamoDB _client;

    public Function() : this(null)
    {
    }

    public Function(IAmazonDynamoDB? client)
    {
        _client = client ?? new AmazonDynamoDBClient();
    }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<Data?> FunctionHandler(Event @event, ILambdaContext context)
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

            var document = Amazon.DynamoDBv2.DocumentModel.Document.FromAttributeMap(getItemResponse?.Item);

            var data = JsonSerializer.Deserialize<Data>(document.ToJson());

            return data;
        }
        catch (Exception exception)
        {
            context.Logger.LogError(exception, "Error");
        }

        return null;
    }
}
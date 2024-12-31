using System.Reflection.Metadata;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Lambda;

public class Function
{

    private const string tableName = "ProductCatalog";
    private AmazonDynamoDBClient _client;
    private DynamoDBContext _dynamoDbContext;
    public Function()
    {
        _client = new AmazonDynamoDBClient();
        _dynamoDbContext = new DynamoDBContext(_client);
    }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<Data> FunctionHandler(Request request, ILambdaContext context)
    {
        context.Logger.LogInformation("Got request {Request}", request);

        var getAction = new GetItemRequest(tableName, new Dictionary<string, AttributeValue>() {
            {
                "ID",
                new AttributeValue {
                    S = request?.PathParameters?.GetValueOrDefault("id")
                 }
             }
        });

        var getItemResponse = await _client.GetItemAsync(getAction);

        context.Logger.LogInformation("Got DynamoDB response {Response}", getItemResponse?.Item);

        var document = Amazon.DynamoDBv2.DocumentModel.Document.FromAttributeMap(getItemResponse?.Item);

        var data = _dynamoDbContext.FromDocument<Data>(document);

        context.Logger.LogInformation("Got Data {Data}", data);

        return data;
    }
}
using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Moq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Lambda.Tests;

public class FunctionTest
{
    [Fact]
    public async Task TestToUpperFunctionAsync()
    {

        string expectedID = "1";
        var mockDynamoDBClient = new Mock<IAmazonDynamoDB>();

        mockDynamoDBClient.Setup(client => client.GetItemAsync(It.IsAny<GetItemRequest>(), default))
            .ReturnsAsync(new GetItemResponse
            {
                Item = new Dictionary<string, AttributeValue>
                {
                    ["ID"] = new AttributeValue
                    {
                        S = expectedID
                    },
                    ["Title"] = new AttributeValue
                    {
                        S = "Title"
                    },
                    ["Rating"] = new AttributeValue
                    {
                        N = "1.0"
                    },
                    ["Genres"] = new AttributeValue
                    {
                        SS = new List<string> { "Genre1", "Genre2" }
                    }
                }
            });

        var function = new Handler(mockDynamoDBClient.Object);

        var context = new TestLambdaContext() { Logger = new TestLambdaLogger() };

        var @event = new Event(new Dictionary<string, string> {
            {
                "id", expectedID
            }
        });

        var response = await function.HandleAsync(@event, context);

        Assert.Equal(expectedID, response?.ID);
    }
}
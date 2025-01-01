using Xunit;
using Amazon.Lambda.TestUtilities;
using Moq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Lambda.Tests;

public class FunctionTest
{
    [Fact]
    public async Task TestToUpperFunction()
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
                    }
                }
            });

        var function = new Function(mockDynamoDBClient.Object);

        var context = new TestLambdaContext() { Logger = new TestLambdaLogger() };

        var @event = new Event(new Dictionary<string, string> {
            {
                "id", expectedID
            }
        });

        var response = await function.FunctionHandler(@event, context);

        Assert.Equal(expectedID, response?.ID);
    }
}

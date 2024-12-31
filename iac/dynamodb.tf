resource "aws_dynamodb_table" "data" {
  name           = "dotnet_data"
  billing_mode   = "PROVISIONED"
  read_capacity  = 1
  write_capacity = 1
  hash_key       = "ID"

  attribute {
    name = "ID"
    type = "S"
  }
}

resource "aws_dynamodb_table_item" "the_matrix" {
  table_name = aws_dynamodb_table.data.name
  hash_key   = aws_dynamodb_table.data.hash_key
  range_key  = aws_dynamodb_table.data.range_key

  item = jsonencode(
    {
      ID    = { S = "1" },
      Title = { S = "The Matrix" },
      Genres = { SS = [
        "Action",
        "Sci-Fi",
        ]
      },
      Rating = { N = "8.7" }
    }
  )
}

resource "aws_dynamodb_table_item" "scott_pilgrim" {
  table_name = aws_dynamodb_table.data.name
  hash_key   = aws_dynamodb_table.data.hash_key
  range_key  = aws_dynamodb_table.data.range_key

  item = jsonencode(
    {
      ID    = { S = "2" },
      Title = { S = "Scott Pilgrim vs. the World" },
      Genres = { SS = [
        "Sci-Fi",
        "Comedy",
        ]
      },
      Rating = { N = "7.5" }
    }
  )
}

resource "aws_dynamodb_table_item" "star_wars" {
  table_name = aws_dynamodb_table.data.name
  hash_key   = aws_dynamodb_table.data.hash_key
  range_key  = aws_dynamodb_table.data.range_key

  item = jsonencode(
    {
      ID    = { S = "3" },
      Title = { S = "Star Wars: Episode IV - A New Hope" },
      Genres = { SS = [
        "Action",
        "Adventure",
        "Fantasy",
        "Sci-Fi",
        ]
      },
      Rating = { N = "8.6" }
    }
  )
}

resource "aws_dynamodb_table_item" "star_wars_v" {
  table_name = aws_dynamodb_table.data.name
  hash_key   = aws_dynamodb_table.data.hash_key
  range_key  = aws_dynamodb_table.data.range_key

  item = jsonencode(
    {
      ID    = { S = "4" },
      Title = { S = "Star Wars: Episode V - The Empire Strikes Back" },
      Genres = { SS = [
        "Action",
        "Adventure",
        "Fantasy",
        "Sci-Fi",
        ]
      },
      Rating = { N = "8.7" }
    }
  )
}

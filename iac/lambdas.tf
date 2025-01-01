locals {
  process_data_lambda_name = "process-data"
  native_aot_lambda_name   = "native-aot"
}

resource "aws_iam_role" "process_data" {
  name               = "${local.process_data_lambda_name}-lambda-role"
  assume_role_policy = data.aws_iam_policy_document.assume_role.json
}

resource "aws_iam_role_policy" "process_data_policies" {
  role   = aws_iam_role.process_data.name
  policy = data.aws_iam_policy_document.process_data_policies.json
}

resource "aws_lambda_function" "lambda" {
  filename      = data.archive_file.dotnet_lambda_archive.output_path
  function_name = local.process_data_lambda_name
  role          = aws_iam_role.process_data.arn
  handler       = "Lambda::Lambda.Function::FunctionHandler"
  runtime       = "dotnet8"
  memory_size   = 512
  timeout       = 30
}

data "archive_file" "dotnet_lambda_archive" {
  type        = "zip"
  source_dir  = "dotnet_lambda_default"
  output_path = "dotnet_lambda_function_payload.zip"
}

data "aws_iam_policy_document" "assume_role" {

  statement {
    effect = "Allow"

    principals {
      type        = "Service"
      identifiers = ["lambda.amazonaws.com"]
    }

    actions = ["sts:AssumeRole"]

  }
}

data "aws_iam_policy_document" "process_data_policies" {
  statement {
    effect = "Allow"
    sid    = "LogToCloudwatch"
    actions = [
      "logs:CreateLogGroup",
      "logs:CreateLogStream",
      "logs:PutLogEvents",
    ]

    resources = ["arn:aws:logs:*:*:*"]
  }

  statement {
    effect = "Allow"

    actions = [
      "dynamodb:GetItem"
      # "dynamodb:Query"
    ]

    resources = [
      aws_dynamodb_table.data.arn
    ]
  }
}

# Native AOT Lambda

resource "aws_iam_role" "native_aot" {
  name               = "${local.native_aot_lambda_name}-lambda-role"
  assume_role_policy = data.aws_iam_policy_document.native_aot_assume_role.json
}

resource "aws_iam_role_policy" "native_aot_policies" {
  role   = aws_iam_role.native_aot.name
  policy = data.aws_iam_policy_document.native_aot_policies.json
}

resource "aws_lambda_function" "native_aot_lambda" {
  filename      = data.archive_file.dotnet_lambda_archive.output_path
  function_name = local.native_aot_lambda_name
  role          = aws_iam_role.native_aot.arn
  handler       = "Lambda" # The handler for AOT is only the assembly (the name in the project file)
  runtime       = "dotnet8"
  memory_size   = 512
  timeout       = 30
}

data "aws_iam_policy_document" "native_aot_assume_role" {

  statement {
    effect = "Allow"

    principals {
      type        = "Service"
      identifiers = ["lambda.amazonaws.com"]
    }

    actions = ["sts:AssumeRole"]

  }
}

data "aws_iam_policy_document" "native_aot_policies" {
  statement {
    effect = "Allow"
    sid    = "LogToCloudwatch"
    actions = [
      "logs:CreateLogGroup",
      "logs:CreateLogStream",
      "logs:PutLogEvents",
    ]

    resources = ["arn:aws:logs:*:*:*"]
  }
}

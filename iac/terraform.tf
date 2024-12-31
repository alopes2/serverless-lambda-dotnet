terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }

  backend "s3" {
    bucket = "terraform-medium-api-notification"
    key    = "serverless-dotnet-lambda/state"
  }
}

# Configure the AWS Provider
provider "aws" {}

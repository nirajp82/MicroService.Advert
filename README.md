
# MicroService.Advert
Build Microservices with .NET 5 & Amazon Web Services using C# , .NET 5, AWS Lambda, DynamoDB, SNS, (CQRS, Circuit Breaker, Service Mesh, API Gateway, HealthCheck etc)
#
#
Using the AWS Credentials File and Credential Profiles (https://docs.aws.amazon.com/sdk-for-php/v3/developer-guide/guide_credentials_profiles.html)
#
1: in run command enter "%UserProfile%" and press enter
#
 Create .aws folder (Path will be like "C:\Users\[USER_NAME]\.aws"
#
#
2: Create file named "credentials" (Please note that there is no file extension")
#
3: Enter access key and secret key
#
The format of the AWS credentials file should look something like the following.
#
#
[default]
#
aws_access_key_id = [aws_access_key_id]
#
aws_secret_access_key = [aws_secret_access_key]
#
#
#
.Net Core Deployment to EC2 from S3 file using Code Deploy
#
1: Launch new EC2 window server and assing role that has following permissions.
#
  1.a: AmazonDynamoDBFullAccess (If your application is using DynamoDB. Give Approprite permission based on application needs)
#
  3.b: Full Access to S3 (or specific Bucket) - So EC2 instance download artifacts (Publish code) from S3
#
2: Install IIS, .net core and Code Deploy Hosting Agent for Window on server
#
3: Create S3 bucket to upload published code.
#
3: Create a service role and assign "AWSCodeDeployRole" permission policy to it.
#
4: Create Code Deployment Application -> Then Code Deployment Group and then -> Create Deployment  
#
  4.1 Assign newly created service role to "Code Deployment Group" so it can deploy code to the EC2 instance/EC2 Auto Scaling Group
#
5: Create build using dotnet publish command. Say it will deploy it to "AdvertAPI" folder
#
6: Add AppSpec.yml file same level as AdvertAPI folder
#
version: 0.0
#
os: windows
#
files:
#
  - source: AdvertAPI
#
    destination: c:\inetpub\wwwroot
#
7: Zip AdvertAPI and AppSepc.yml file 
#
8: Upload code to s3 bucket
#
9: Run deployment
#

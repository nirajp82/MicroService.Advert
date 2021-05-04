# MicroService.Advert
Using the AWS Credentials File and Credential Profiles (https://docs.aws.amazon.com/sdk-for-php/v3/developer-guide/guide_credentials_profiles.html)
1: in run command enter "%UserProfile%" and press enter
 Create .aws folder (Path will be like "C:\Users\[USER_NAME]\.aws"
2: Create file named "credentials" (Please note that there is no file extension")
3: Enter access key and secret key

The format of the AWS credentials file should look something like the following.
[default]
aws_access_key_id = [aws_access_key_id]
aws_secret_access_key = [aws_secret_access_key]

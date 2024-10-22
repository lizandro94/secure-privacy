# secure-privacy
.NET/C# web application with a basic Angular front-end, integrating MongoDB, and considering GDPR compliance aspects.

**Built with:**

 - .Net 8
 - Node JS 20.18.0
 - Angular 18.2.9
 - MongoDB 7.0.14

## Getting Started
**Installation**
Clone the repo:

    git clone https://github.com/lizandro94/secure-privacy.git

Make sure your mongo database is running on 

> localhost:27017

**.Net Project**

Navigate to the .Net project

    cd secure-privacy/webapi
  
  Build the .Net project
  
    dotnet build
Run the .Net project

    dotnet run
Make sure the API is running on 

> localhost:5168

Otherwise, you will have to update the angular app environment file:

> src/environments/environments.ts

**Angular Project**
First, make sure you are using the correct Node JS version (20.18.0). In the same repo navigate to the angular project, if you are in the root of the repo you need to run:

    cd web-app
Install dependencies

    npm install
Run the angular project

    npm start
Now you are ready to test the app on:

> localhost:4200/

Please click on "Register" to create your user, these are the app features:

 - Register User
 - Login
 - Create products
 - List products
 - Get user details (Account page)
 - Delete user account (Account page)
 - Logout

## GDPR compliance measures
Some of the measures you will find in the project:

 - User Data is encrypted in the database
 - User can delete his account (Right to be forgotten)
 - User must accept terms and conditions to create his account

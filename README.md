# Background


RAPID stands for REST API for Database. 

I have created docker images which have been uploaded to DockerHub. 

These image provide a RESTful API to SQL and MySql databases. 


The following operations are supported:-


1. Query a table.

2. Add records to a table.

3. Update existing records in a table.

4. Delete records from a table.



## Installing the API




Install docker Quickstart Terminal on your machine. Open Quickstart Terminal.


Pull the docker image from DockerHub:

*$ docker pull workmaze/rapid.mysql*
*$ docker pull workmaze/rapid.sql*


Run the docker image

*$ docker run -p {yourportumber}:8080 -e RAPID_MYSQLCONNECTIONSTRING="{mysqlconnectionstring}" workmaze/rapid.mysql*

*$ docker run -p {yourportumber}:8080 -e RAPID_SQLCONNECTIONSTRING="{sqlconnectionstring}" workmaze/rapid.sql*

## Using the API


Open the MySql work-bench to create the following table:-


*CREATE TABLE `user` (`idUser` int(11) NOT NULL AUTO_INCREMENT,  
`Name` varchar(45) DEFAULT NULL,  
`Country` varchar(45) DEFAULT NULL, 
`Language` varchar(45) DEFAULT NULL, 
`Age` int(11) DEFAULT NULL,  
`MoreInfo` json DEFAULT NULL,  
PRIMARY KEY (`idUser`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;*


Open SQL Management Console to create the following table:-

*CREATE TABLE [dbo].[User]
(
[idUser] [int] IDENTITY(1,1) NOT NULL,
[Name] nvarchar NULL,
[Country] nvarchar NULL,
[Language] nvarchar NULL,
[Age] [int] NULL,
[MoreInfo] [xml] NULL
)*

a## Add records to a table


POST {url}/api/table/{tablename}

Body : Array of JSON objects corresponding to the table schema.



*POST http://192.168.99.100:80/api/table/user*


*[{
"Country" : "UK",
"Age" : "30",
"Name" : "Simon",
"Language" : "Welsh"
},
{
"Country" : "Sweden",
"Age" : "30",
"Name" : "Philip",
"Language" : "Swedish"
},
{
"Country" : "Norway",
"Age" : "40",
"Name" : "Ola",
"Language" : "Norsk"
},
{
"Country" : "Finland",
"Age" : "25",
"Name" : "Ola",
"Language" : "Finnish"
}]*



Returns 204



## Update existing records in a table


PUT {url}/api/table/{tablename}?{conditions}
Body : JSON object corresponding to the fields to update.



*PUT http://192.168.99.100:80/api/table/user?name=Simon&country=uk*


*{
"Country" : "Wales",
"Age" : "35"
Language" : "English"
}*



Returns 204



## Delete records from a table


DELETE {url}/api/table/{tablename}?{conditions}



*DELETE http://192.168.99.100:80/api/table/user?name=Simon&country=wales*



Returns 204



## Query a table



GET {url}/api/table/{tablename}?{conditions}

*GET http://192.168.99.100:80/api/table/user?name=Ola*



Retruns 200

Body : JSON object containing the result from the query.


*[{
"Country" : "Norway",
"Age" : "40",
"Name" : "Ola",
"Language" : "Norsk"
},
{
"Country" : "Finland",
"Age" : "25",
"Name" : "Ola",
"Language" : "Finnish"
}]*




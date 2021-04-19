# Realworld one Backend Developer Test

# Upside-down Kitten Generator

# Task :

Create a RESTful API that generates upside-down kitten pictures

1. The API should fetch cat images from the Cat as a service API (https://cataas.com) and flip the image upside down.
2. The endpoint should require some form of user authentication to authorize its use.
3. The API should provide some method to register new users and login said users.
4. Users should be stored in a database (in memory is fine).

#### You could make the endpoints like so:

• An endpoint to show the upside-down cat. Accessing in the browser should display the image directly without requiring download.
• An endpoint to create a new user (registration)
• An endpoint to get user information
• Endpoints to handle authentication, login, logout, etc

### Extensions

The task is purposely straightforward. It could make sense to add some features which you think make the API more useful. You might consider:
• Adding some form of API documentation (e.g. handwritten, generated, live, etc)
• Adding different endpoints which return different types of images which are manipulated in different ways.
• Support more than one type of authentication (basic, bearer, OAuth, etc)
• Anything else you think might be important!

### Note
Please include a readme file which outlines your decision-making process while undertaking the test.


# My Solution:


I have started with creating the User entity, then The Login and User controllers.

I have used EntityFrameworkCore to create the database.

Before running the project please execute **update-database** first.

I have used the Automapper to map the DTOs (Data Transfer Objects)to the model objects (in my case entities with the Models).

I have used swagger to ensure documentation and graphic access to the API endpoints.

I have used basic authentication and bearer authentication. You can find the configuration in the Startup file.

I have also added a BasicAuthentcation attribute to use it in case I want a basic auth for my endpoint.

To ensure the authentication using the Jwt Token, please execute the Login endpoint. Then copy the token. Click on Authorize button situated in the right top corner and paste the Token Key in its dedicated TextBox.

For the Image flipping :

I have injected the HttpClientFactory in the startup project.

I have created two classes : ImageLoader for reading the stream from the URL and ImageHandler for all the manipulation that we can exercice to the Image:

When calling the FlipAndRotate endpoint, it requires a rotation Type (int). The default value is 1 for the flip image.

Here is all the types of rotations associted with rotation types:

  1 = Rotate180FlipNone
  
  2 = Rotate180FlipX
  
  3 = Rotate180FlipXY
  
  4 = Rotate180FlipY
  
  5 = Rotate270FlipNone
  
  6 = Rotate270FlipX
  
  7 = Rotate270FlipXY
  
  8 = Rotate270FlipY
  
  9 = Rotate90FlipNone
  
  10 = Rotate90FlipX
  
  11 = Rotate90FlipXY
  
  12 = Rotate90FlipY
  
  13 = RotateNoneFlipNone
  
  14 = RotateNoneFlipX
  
  15 = RotateNoneFlipXY
  
  16 = RotateNoneFlipY
  
  
  I did not finish all the possible unit tests, but I have started with one or more test for each controller . I have used Moq.




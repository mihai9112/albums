# RunPath Web API
Simple Web API for .NET Core consuming JsonPlaceHolder Api

To run, navigate to the root directory and input: `docker-compose up --build -d`. This will run the web api into a docker container, available at `http://localhost:5050`.

If you don't have docker installed please see [how to install Docker Desktop](https://www.docker.com/products/docker-desktop)

The API is designed followint Level 3 REST using Hypermedia links. Please have a look and the [REST Maturity Model](https://martinfowler.com/articles/richardsonMaturityModel.html) posted by Marting Fowler.

The project can also be opened in your editor of your choosing and run it in debug mode, the port will be assigned automatically.

Once you have the docker container up and running, using Postman ping the endpoint: GET `http://localhost:5050`. The following response will be returned:

```
{
    "links": {
        "self": {
            "href": "http://localhost:5050/"
        },
        "albums-get": {
            "href": "http://localhost:5050/albums"
        }
    }
}
```

This response is telling the client, how to navigate the web api. The `self` link tells what link you've accessed and the `albums-get` link tells the possible route, in order to get all the available albums

Let's explore our API further! Accessing the link: GET `http://localhost:5050/albums` gives us this response, this is just a subset:

```
{
  "response_data": {
    "id": "1",
    "userId": "1",
    "title": "quidem molestiae enim"
  },
  "_links": {
      "self": {
          "href": "http://localhost:5050/albums"
      },
      "photos-get": {
          "href": "http://localhost:5050/albums/1/photos"
      },
      "user-album-get": {
          "href": "http://localhost:5050/users/1/albums"
      }
  }
}
```
The response is giving us back the albums available. In the response data we have, details such as `id` of the album, `userId` and `title`. The links sections, as in the previous example, tells us what routes are available for the client to explore combined with the self link which was access in order to get here.

Let's explore the `user-album-get` link by calling the endpoint: GET `http://localhost:5050/users/1/albums`. Yet again this is just a subset of the available data.

```
{
  "response_data": {
    "userId": "1"
  },
  "albums": [
    {
      "user_id": 1,
      "id": 1,
      "title": "quidemmolestiaeenim",
      "photos": [
        {
          "album_id": 1,
          "id": 1,
          "title": "accusamusbeataeadfaciliscumsimiliquequisunt",
          "url": "https:test-placeholder",
          "thumbnail_url": "https:test-placeholder"
        }
      ]
    }
  ],
  "_links": {
    "self": {
      "href": "http://localhost/users/1/albums"
    }
  }
}
```
This endpoint gets us all the albums, with or without photos available to a specified user. It aggregates around the userId in the endpoint.






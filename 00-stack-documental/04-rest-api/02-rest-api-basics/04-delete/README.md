# 04 Delete method

In this example we are going to implement a `delete` method using http server in Nodejs.

We will start from `03-put`.

# Steps to build it

Finally, we could implement the method to delete a discontinued book:

_./index.mjs_

```diff
import http from "http";
import {
  getBookList,
  getBook,
  insertBook,
  updateBook,
+ deleteBook,
} from "./mock-db.mjs";

const handleRequest = (req, res) => {
...
    } else if (method === "PUT") {
...
-   }
+   } else if(method === "DELETE") {
+     deleteBook(bookId);
+     res.statusCode = 204;
+     res.end();
+   }
  } else {
    res.write("My awesome books portal");
    res.end();
  }
};
...
```

Run app:

```bash
node index.mjs

```

The request:

```
URL: http://localhost:3000/api/books/1
METHOD: DELETE
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).

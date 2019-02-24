const express = require("express");
const path = require("path");
const flash = require("connect-flash");
const routes = require("./routes/index");
const errorHandlers = require("./handlers/errorHandlers");
const axios = require("axios");

// Setup default value for axios
axios.defaults.baseURL = `${process.env.TARGET_SITE}/api/v4/`;
axios.defaults.headers.common["Private-Token"] = process.env.PRIVATE_TOKEN;

// create our Express app
const app = express();

app.set("views", path.join(__dirname, "views")); // this is the folder where we keep our pug files
app.set("view engine", "pug"); // we use the engine pug, mustache or EJS work great too

// serves up static files from the public folder. Anything in public/ will just be served up as the file it is
app.use(express.static(path.join(__dirname, "public")));

// // The flash middleware let's us use req.flash('error', 'Shit!'), which will then pass that message to the next page the user requests
app.use(flash());

// After allllll that above middleware, we finally handle our own routes!
app.use("/", routes);

// If that above routes didnt work, we 404 them and forward to error handler
app.use(errorHandlers.notFound);

// One of our error handlers will see if these errors are just validation errors
app.use(errorHandlers.flashValidationErrors);

// Otherwise this was a really bad error we didn't expect! Shoot eh
if (app.get("env") === "development") {
  /* Development Error Handler - Prints stack trace */
  app.use(errorHandlers.developmentErrors);
}

// production error handler
app.use(errorHandlers.productionErrors);

// done! we export it so we can start the site in start.js
module.exports = app;

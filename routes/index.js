const express = require("express");
const router = express.Router();
const projectController = require("../controllers/projectController");
const { catchErrors } = require("../handlers/errorHandlers");

router.get("/api/getProjects", projectController.getProjects);

module.exports = router;

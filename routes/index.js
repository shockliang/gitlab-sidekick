const express = require("express");
const router = express.Router();
const projectController = require("../controllers/projectController");
const groupController = require("../controllers/groupController");
const { catchErrors } = require("../handlers/errorHandlers");

// projects
router.get("/api/projects", projectController.getProjects);

// groups
router.get("/api/groups", groupController.getGroups);
router.get("/api/groups/:id", groupController.getGroupById);

module.exports = router;

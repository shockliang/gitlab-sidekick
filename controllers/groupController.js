const axios = require("axios");

exports.getGroups = (req, res) => {
  axios
    .get("/groups?statistics=true&all_available=true")
    .then(response => {
      console.log(response.headers["x-total"]);
      res.json(response.data);
    })
    .catch(err => {
      console.error(err);
    });
};

exports.getGroupById = (req, res) => {
  const id = req.params.id;

  axios
    .get(`/groups/${id}`)
    .then(response => {
      res.json(response.data);
    })
    .catch(console.error);
};

const axios = require("axios");

exports.getProjects = (req, res) => {
  const perPage = req.query.prePage || 10;

  axios
    .get("/projects", {
      params: {
        per_page: perPage
      }
    })
    .then(response => {
      console.log(response.headers["x-total"]);
      res.json(response.data);
    })
    .catch(err => {
      console.error(err);
    });
};

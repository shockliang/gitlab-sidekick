const axios = require("axios");

exports.getProjects = (req, res) => {
  const perPage = req.params.prePage || 10;
  console.log(req.params);
  axios
    .get(
      `${
        process.env.TARGET_SITE
      }/api/v4/projects?per_page=${perPage}&private_token=${
        process.env.PRIVATE_TOKEN
      }`
    )
    .then(response => {
      console.log(response.headers["x-total"]);
      res.json(response.data);
    })
    .catch(err => {
      console.error(err);
    });
};

const axios = require("axios");

exports.getProjects = (req, res) => {
  axios
    .get(
      `${process.env.TARGET_SITE}/api/v4/projects?per_page=1&private_token=${
        process.env.PRIVATE_TOKEN
      }`
    )
    .then(response => {
      console.log(response.headers['x-total']);
      res.json(response.data);
    })
    .catch(err => {
      console.error(err);
    });
  
};

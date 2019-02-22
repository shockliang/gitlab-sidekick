const axios = require("axios");

exports.getGroups = (req, res) => {
  axios
    .get(
      `${
        process.env.TARGET_SITE
      }/api/v4/groups?statistics=true&all_available=true&private_token=${
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

import app from "./config/app";

const PORT = process.env.PORT || 8081;

app.listen(PORT, () => {
    console.log("Webapp listening on port : " + PORT);
})
import express from 'express';
import bodyParser from 'body-parser';
import morgan from 'morgan';
import cookieParser from 'cookie-parser';
import session from 'express-session';

import { Routes } from "./routes";

class App {
    public app: express.Application;
    public routePrv: Routes = new Routes;

    constructor() {
        this.app = express();
        this.config();
        this.routePrv.routes(this.app);
    };

    private config(): void {
        this.app.use(express.static('./public'));

        this.app.use(bodyParser.json());
        this.app.use(bodyParser.urlencoded({extended: false}));

        this.app.use(morgan('dev'));

        this.app.use(session({ secret: "this is a secret i think" }));
        this.app.use(cookieParser());

        this.app.set('view engine', 'ejs');
    };
}

export default new App().app;
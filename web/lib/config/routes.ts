import { Request, Response } from 'express';

import { UserController } from '../controller/user.controller';

export class Routes {
    public userController: UserController = new UserController;

    public routes(app): void{
        app.route('/').get(this.userController.home);
        app.route('/').post(this.userController.signin_post);

        app.route('/signin').get(this.userController.signin);
        app.route('/signin').post(this.userController.signin_post);

        app.route('/signup').get(this.userController.signup);
        app.route('/signup').post(this.userController.signup_post);

        app.route('/dashboard').get(this.userController.dashboard);

        app.route('/service/:id').get(this.userController.service);
        app.route('/service/:id').post(this.userController.connect_post);
        app.route('/service/disconnect/:id').get(this.userController.disconnect_post);
        app.route('/actions/:id/:action').post(this.userController.actions);
        app.route('/actions/:id/:action').get(this.userController.actions);
        app.route('/reactions/:id/:reaction').get(this.userController.reactions);

        app.route('/profil').get(this.userController.profil);
        app.route('/profil').post(this.userController.profil_post);

        app.route('/logout').get(this.userController.logout);

        app.route('*').get(this.userController.dashboard);
    }
};
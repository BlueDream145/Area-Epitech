import { Request, Response } from 'express';
import JSON from 'circular-json';
import axios from 'axios';

import cookieParser from 'cookie-parser';

export class UserController {
    public home(req: Request, res: Response) {
        res.render('home');
    }

    public signin(req: Request, res: Response) {
        res.render('signin', {message: ' '});
    };

    public signin_post(req: Request, res: Response) {
        console.log('Result = ' + JSON.stringify(req.body));
        let _username: string = req.body.username;
        let _password: string = req.body.password;
        axios.post('http://server:8080/identificationmessage?username=' + _username + "&password=" + _password)
        .then(result => {
            if (JSON.stringify(result.data.Result) == '2') {
                console.log('Result = User unknow');
                res.render('signin', { message: 'User unknow' });
            } else if (JSON.stringify(result.data.Result) == '1') {
                let _token: any = result.data.Token;
                req.app.set('username', result.data.Username);
                req.app.set('name', result.data.Name);
                req.app.set('mail', result.data.Mail);
                req.app.set('services', result.data.Services);
                req.app.set('password', req.body.password);
                res.cookie("token", _token).redirect('/dashboard');
            }
        }).catch(err => {
            console.log('Erreur = ' + JSON.stringify(err));
        })
    }

    public signup(req: Request, res: Response) {
        res.render('signup', { message: ' ' });
    };

    public signup_post(req: Request, res: Response) {
        console.log('Result = ' + res);
        let _email: string = req.body.email;
        let _password: string = req.body.password;
        let _username: string = req.body.username;
        let _name: string = req.body.name;
        axios.post('http://server:8080/registermessage?username=' + _username + "&password=" + _password + '&name=' + _name + '&mail=' + _email)
        .then(result => {
            console.log('Result = ' + JSON.stringify(result));
            if (result.status == 200) {
                if (JSON.stringify(result.data.Result) == '1') {
                    res.redirect('/signin');
                } else if (JSON.stringify(result.data.Result) == '6') {
                    res.render('signup', { message: 'Invalid Password' });
                } else if (JSON.stringify(result.data.Result) == '5') {
                    res.render('signup', { message: 'Invalid Email' });
                } else if (JSON.stringify(result.data.Result) == '4') {
                    res.render('signup', { message: 'Invalid Username' });
                } else if (JSON.stringify(result.data.Result) == '3') {
                    res.render('signup', { message: 'Mail already registered' });
                } else if (JSON.stringify(result.data.Result) == '2') {
                    res.render('signup', { message: 'Username already registered' });
                } else if (JSON.stringify(result.data.Result) == '7') {
                    res.render('signup', { message: 'Error' });
                }
            }
        }).catch(err => {
            console.log('Erreur = ' + err);
        })
    }

    public logout(req: Request, res: Response) {
        if (req.session) {
            req.session.destroy((err) => {
                if (err) {
                    return console.log(err);
                }
                res.redirect('/');
            });
        }
    }

    public dashboard(req: Request, res: Response) {
        let _token = req.cookies.token;
        if (!_token) {
            res.redirect('/');
        } else if (!req.app.get('username')) {
            res.redirect('/');
        } else {
            res.render('dashboard', { user: req.app.get('username'), service: req.app.get('services')});
        }
    }

    public profil(req: Request, res: Response) {
        res.render('profil', { user: ' ', username: req.app.get('username'), name: req.app.get('name'), mail: req.app.get('mail')});
    }

    public profil_post(req: Request, res: Response) {
        console.log('Result = ' + res);
        let _email: string = req.body.email.length == 0 ? req.app.get('mail') : req.body.email;
        let _password: string = req.body.password.length == 0 ? req.app.get('password') : req.body.password;
        let _username: string = req.body.username.length == 0 ? req.app.get('username') : req.body.username;
        let _name: string = req.body.name.length == 0 ? req.app.get('name') : req.body.name;
        axios.post('http://server:8080/profileupdaterequestmessage?username=' + _username + '&name=' + _name + '&mail=' + _email + "&password=" + _password + '&token=' + req.cookies.token)
            .then(result => {
                console.log('Result = ' + JSON.stringify(result.data)   );
                if (result.status == 200) {
                    if (JSON.stringify(result.data.Result) == '1') {
                        if (req.body.email.length != 0) {req.app.set('mail', req.body.email); }
                        if (req.body.password.length != 0) {req.app.set('password', req.body.password); }

                        if (req.body.username.length != 0) {req.app.set('username', req.body.username); }
                        if (req.body.name.length != 0) {req.app.set('name', req.body.name); }
                        res.render('profil', { message: ' ', username: req.app.get('username'), name: req.app.get('name'), mail: req.app.get('mail') });
                    } else if (JSON.stringify(result.data.Result) == '6') {
                        res.render('profil', { message: 'Invalid Password', username: req.app.get('username'), name: req.app.get('name'), mail: req.app.get('mail')});
                    } else if (JSON.stringify(result.data.Result) == '5') {
                        res.render('profil', { message: 'Invalid Email', username: req.app.get('username'), name: req.app.get('name'), mail: req.app.get('mail')});
                    } else if (JSON.stringify(result.data.Result) == '4') {
                        res.render('profil', { message: 'Invalid Username', username: req.app.get('username'), name: req.app.get('name'), mail: req.app.get('mail')});
                    } else if (JSON.stringify(result.data.Result) == '3') {
                        res.render('profil', { message: 'Mail already registered', username: req.app.get('username'), name: req.app.get('name'), mail: req.app.get('mail')});
                    } else if (JSON.stringify(result.data.Result) == '2') {
                        res.render('profil', { message: 'Username already registered', username: req.app.get('username'), name: req.app.get('name'), mail: req.app.get('mail')});
                    } else if (JSON.stringify(result.data.Result) == '7') {
                        res.render('profil', { message: 'Error', username: req.app.get('username'), name: req.app.get('name'), mail: req.app.get('mail')});
                    }
                }
            }).catch(err => {
                console.log('Erreur = ' + err);
            })
    }


    public service(req: Request, res: Response) {
        res.render('service', { user: req.app.get('username'), service: req.app.get('services'),
                                id: req.params.id , action: '0', data: null});
    };

    public connect_post(req: Request, res: Response) {
        let token = req.cookies.token;
        if (!token) {
            console.log('Token null');
        } else {
            console.log('Result = ' + JSON.stringify(req.body));
            let url = 'http://server:8080/registerservicemessage?serviceid=' + req.params.id + '&username=' + req.body.username + "&password=" + req.body.password + '&token=' + token;
            console.log('URL = ' + url);
            axios.post(url)
                .then(result => {
                    if (JSON.stringify(result.data.MessageId) == '8') {
                        console.log('Result = service connected');
                        req.app.set('services', result.data.Services);
                        res.render('service', { user: req.app.get('username'), service: req.app.get('services'),
                                                id: req.params.id, action: '0', data: null });
                     } else {
                        res.render('service', { user: req.app.get('username'), service: req.app.get('services'),
                                                id: req.params.id, action: '0', data: null });
                    }
                }).catch(err => {
                    console.log('Erreur = ' + JSON.stringify(err));
                })
        }
    };

    public disconnect_post(req: Request, res: Response) {
        let token = req.cookies.token;
        if (!token) {
            console.log('Token null');
        } else {
            let url = 'http://server:8080/deleteservicemessage?serviceid=' + req.params.id + '&token=' + token;
            console.log('URL = ' + url);
            axios.post(url).then(result => {
                    if (JSON.stringify(result.data.MessageId) == '8') {
                        console.log('Result = service disconnected');
                        req.app.set('services', result.data.Services);
                        res.render('dashboard', { user: req.app.get('username'), service: req.app.get('services') });
                    } else {
                        res.render('service', {
                            user: req.app.get('username'), service: req.app.get('services'),
                            id: req.params.id, action: '0', data: null
                        });
                    }
                }).catch(err => {
                    console.log('Erreur = ' + JSON.stringify(err));
                })
        }
    };

    public actions(req: Request, res: Response) {
        let token = req.cookies.token;
        let url = 'http://server:8080/actionrequestmessage?actionid=' + req.params.action + '&token=' + token;
        if (!token) {
            console.log('Token null');
        } else {
            if (req.body.message != undefined) {
                url = 'http://server:8080/actionrequestmessage?actionid=' + req.params.action + '&token=' + token + '&params=' + req.body.params + '|' + req.body.content + '|' + req.body.message;
            } else if (req.body.content != undefined) {
                url = 'http://server:8080/actionrequestmessage?actionid=' + req.params.action + '&token=' + token + '&params=' + req.body.params + '|' + req.body.content;
            }
            else if (req.body.params != undefined) {
                url = 'http://server:8080/actionrequestmessage?actionid=' + req.params.action + '&token=' + token + '&params=' + req.body.params;
            }
            console.log('URL = ' + url);
            axios.post(url)
                .then(result => {
                    console.log(result.data.Output);
                    if (req.params.action == 12 && result.data.Output == "True") {
                        res.render('service', { user: req.app.get('username'), service: req.app.get('services'),
                            id: req.params.id, action: req.params.action, data: "mail envoyé"});
                    }
                    if (JSON.stringify(result.data.Result) == '2') {
                        res.render('service', { user: req.app.get('username'), service: req.app.get('services'),
                                                id: req.params.id, action: req.params.action, data: JSON.parse(result.data.Output)});
                    } else {
                        res.render('service', { user: req.app.get('username'), service: req.app.get('services'),
                                                 id: req.params.id, action: '0', data: null});
                    }
                }).catch(err => {
                    console.log('Erreur = ' + JSON.stringify(err));
                })
        }
     };

    public reactions(req: Request, res: Response) {
        let token = req.cookies.token;
        let url = 'http://server:8080/reactionrequestmessage?reactionid=' + req.params.reaction + '&token=' + token;
        if (!token) {
            console.log('Token null');
        } else {
            console.log('Result = ' + JSON.stringify(req.body));
            console.log(req.params.id);
            console.log('URL = ' + url);
            axios.post(url)
                .then(result => {
                    if (JSON.stringify(result.data.Result) == '2') {
                        console.log('Result = reaction success');
                        console.log('Result = ' + JSON.stringify(result.data.Output));
                        res.render('service', { user: req.app.get('username'), service: req.app.get('services'),
                                                id: req.params.id, action: '0', data: result.data.Output });
                    } else {
                        res.render('service', { user: req.app.get('username'), service: req.app.get('services'),
                                                 id: req.params.id, action: '0', data: null });
                    }
                }).catch(err => {
                    console.log('Erreur = ' + JSON.stringify(err));
                })
        }
    };
}
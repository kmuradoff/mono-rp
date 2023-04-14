let authWindow;

mp.events.add("authRegister",(login, email, password) => {
    mp.events.callRemote("authOnRegister", login, email, password);
} );

mp.events.add("authLogin", (login, password) => {
    mp.events.callRemote("authOnLogin", login, password);
})

mp.events.add("sendTextError", (text) => {
    authWindow.execute(`sendError("${text}")`);
})

mp.events.add("showAuthWindow", () => {
    authWindow = mp.browsers.new("package://web/auth/index.html");


    setTimeout(() =>{
        mp.gui.cursor.show(true, true);
        mp.game.ui.displayHud(false);
        mp.gui.chat.show(false);
        mp.game.ui.displayRadar(false);
    }, 200);
});

mp.events.add('closeAuthWindow', () => {
    mp.gui.cursor.show(false, false);
    mp.game.ui.displayHud(true);
    mp.gui.chat.show(true);
    mp.game.ui.displayRadar(true);

    if(authWindow != null){
        authWindow.destroy();
        authWindow = null;
    }
})


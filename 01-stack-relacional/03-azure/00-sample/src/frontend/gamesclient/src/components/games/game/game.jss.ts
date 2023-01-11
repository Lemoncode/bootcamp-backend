import { createUseStyles } from "react-jss";

export const GameStyles = createUseStyles((theme: any) => ({
    game: {
        display: "flex",
        flexFlow: "row",
        justifyContent: "stretch",
        alignItems: "center",
        paddingRight: '20px',
        paddingLeft: '0',
        columnGap: '10px',
        color: '#190649',
        fontWeight: "bold",
        background: '#E2DCF8',
    },
    bullet: {
        width: '16px',
        height: '128px',
        background: '#3A0CA3'
    }
}));
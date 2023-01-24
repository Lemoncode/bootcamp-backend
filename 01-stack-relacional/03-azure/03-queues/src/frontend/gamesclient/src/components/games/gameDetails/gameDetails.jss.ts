import { createUseStyles } from "react-jss";

export const GameDetailsStyles = createUseStyles((theme: any) => ({
    container: {
        display: "flex",
        rowGap: '10px',
        columnGap: '10px',
        padding: '20px',
    },
    cover: {
        height: '256px'
    },
    screenshots: {
        display: "flex",
        height: "100px",
        columnGap: "10px",
        padding: "10px",
        width: "100%",
        background: "#f7f7f7",
        border: "1px solid #3A0CA3"
    }
}));
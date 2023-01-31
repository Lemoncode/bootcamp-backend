import { createUseStyles } from "react-jss";

export const GamesContainerStyles = createUseStyles((theme: any) => ({
    page: {
        padding: '20px'
    },
    healthCheckOk: {
        color: 'green',
        fontWeight: "bold"
    },
    healthCheckError: {
        color: 'red',
        fontWeight: "bold"
    },
    container: {
        display: "flex",
        flexFlow: "row wrap",
        justifyContent: "stretch",
        rowGap: "5px",
        columnGap: "10px",
        width: '100%',
        padding: '20px 0',

        '& > *': {
            flex: '1 1 250px'
        }
    }
}));
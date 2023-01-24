import { useEffect } from "react";
import { useTheme } from "react-jss";
import { useSelector } from "react-redux";
import { IGame } from "../../models/IGame";
import { getGamesAction } from "../../redux/actions/games";
import { useAppDispatch } from "../../redux/hooks";
import { gamesSelector, getGamesSelector } from "../../redux/selectors/games";
import { GameItem } from "./gameItem/gameItem";
import { GamesContainerStyles } from "./games.jss";

export const GamesContainer = () => {
    const theme = useTheme();
    const styles = GamesContainerStyles({ ...theme });
    const dispatch = useAppDispatch();
    const games = useSelector(getGamesSelector.selectAll);
    const { isLoading, hasErrors, errorMessage } = useSelector(gamesSelector);

    useEffect(() => {
        dispatch(getGamesAction());
    }, [dispatch])

    return (
        <>
            <div className={styles.page}>
                <div className={hasErrors ? styles.healthCheckError : styles.healthCheckOk}>
                    Estado: {isLoading ? "Loading" : "Loaded"}
                    <br /> Errores: {hasErrors ? errorMessage : "Sin errores "}
                </div>
                <div className={styles.container}>
                    {games.map((item: IGame, index: number) => {
                        return (
                            <GameItem key={index} game={item} />
                        )
                    })}
                </div>
            </div>
        </>
    )
}
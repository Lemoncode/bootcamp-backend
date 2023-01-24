import { useTheme } from "react-jss";
import { Link } from "react-router-dom";
import { IGame } from "../../../models/IGame"
import { GameItemStyles } from "./gameItem.jss";

interface IGameProps {
    game: IGame
}

export const GameItem = (props: IGameProps) => {
    const theme = useTheme();
    const styles = GameItemStyles({ ...theme });

    return (
        <>
            <article className={styles.game}>
                <div className={styles.bullet}>
                    <img className={styles.cover} 
                        src={props.game.posterUrl} 
                        alt={props.game.title} />
                </div>
                {props.game.title}
                <Link to={"Game/" + props.game.id}>Ver</Link>
            </article>
        </>
    )
}


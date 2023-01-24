import { useTheme } from "react-jss";
import { IGame, IScreenshot } from "../../../models/IGame"
import { GameDetailsStyles } from "./gameDetails.jss";
import { useParams } from 'react-router';
import { useAppSelector } from "../../../redux/hooks";
import { gameByIdSelector } from "../../../redux/selectors/games";
import { Uploader } from "../../common/uploader/uploader";

export const GameDetails = () => {
    const theme = useTheme();
    const styles = GameDetailsStyles({ ...theme });
    const { id } = useParams();
    const game: IGame = useAppSelector(gameByIdSelector(id))!;

    // useEffect(() => {
    //     dispatch(getGameAction());
    // }, [dispatch])

    return (
        <>
            <section className={styles.container}>
                <div><img className={styles.cover} src={game.posterUrl} alt={game.title} /></div>
                <div>
                    <header><h1>{game.title}</h1></header>
                    <div className={styles.screenshots}>
                        {game.screenshots.map((item: IScreenshot, index: number) => {
                            return (
                                <img key={index} src={item.url} alt={item.filename} />
                            )
                        })}
                    </div>
                </div>
            </section>
            <div>
                <Uploader id={id!} />
            </div>
        </>
    )
}